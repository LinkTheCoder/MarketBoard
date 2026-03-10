/**
 * Annonssystem Integration - JavaScript exempel
 * Detta exempel visar hur man integrerar med Subscribers Web API
 * för att skapa ett annonssystem som fungerar enligt kraven.
 */

class AnnouncementSystemIntegration {
    constructor() {
        // Konfigurera API URL - ändra till din server
        this.apiBaseUrl = 'https://localhost:7200/api';
    }

    /**
     * Hämtar prenumerantinformation från API:et
     * @param {string} subscriptionNumber - Prenumerationsnummer
     * @returns {Promise<Object|null>} Prenumerantdata eller null om inte hittad
     */
    async getSubscriberInfo(subscriptionNumber) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/subscribers/by-subscription/${subscriptionNumber}`);
            
            if (response.ok) {
                const subscriber = await response.json();
                console.log('Prenumerant hittad:', subscriber);
                return subscriber;
            } else if (response.status === 404) {
                console.log('Prenumerant inte hittad');
                return null;
            } else {
                throw new Error(`API error: ${response.status}`);
            }
        } catch (error) {
            console.error('Fel vid hämtning av prenumerant:', error);
            throw error;
        }
    }

    /**
     * Skapar en ny annons via API:et
     * @param {Object} advertisementData - Annonsdata
     * @returns {Promise<Object>} Skapad annons
     */
    async createAdvertisement(advertisementData) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/advertisements`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(advertisementData)
            });

            if (response.ok) {
                const result = await response.json();
                console.log('Annons skapad:', result);
                return result;
            } else {
                const errorText = await response.text();
                throw new Error(`Fel vid skapande av annons: ${errorText}`);
            }
        } catch (error) {
            console.error('Fel vid skapande av annons:', error);
            throw error;
        }
    }

    /**
     * Hämtar alla publicerade annonser
     * @returns {Promise<Array>} Lista över annonser
     */
    async getPublishedAdvertisements() {
        try {
            const response = await fetch(`${this.apiBaseUrl}/advertisements/published`);
            
            if (response.ok) {
                const advertisements = await response.json();
                console.log(`${advertisements.length} annonser hämtade`);
                return advertisements;
            } else {
                throw new Error(`API error: ${response.status}`);
            }
        } catch (error) {
            console.error('Fel vid hämtning av annonser:', error);
            throw error;
        }
    }

    /**
     * Beräknar annonspris baserat på användartyp
     * @param {number} advertiserType - 1 för prenumerant, 2 för företag
     * @returns {Promise<number>} Priset
     */
    async calculateAdvertisementPrice(advertiserType) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/advertisements/calculate-price/${advertiserType}`);
            
            if (response.ok) {
                const price = await response.json();
                return price;
            } else {
                throw new Error(`API error: ${response.status}`);
            }
        } catch (error) {
            console.error('Fel vid prisberäkning:', error);
            throw error;
        }
    }

    /**
     * Exempel på komplett arbetsflöde för prenumerant
     */
    async subscriberWorkflowExample() {
        console.log('=== PRENUMERANT EXEMPEL ===');
        
        // Steg 1: Hämta prenumerantinfo
        const subscriber = await this.getSubscriberInfo('SUB001');
        
        if (!subscriber) {
            console.log('Prenumerant hittades inte');
            return;
        }

        // Steg 2: Beräkna pris (ska vara 0 för prenumeranter)
        const price = await this.calculateAdvertisementPrice(1);
        console.log(`Annonspris för prenumerant: ${price} kr`);

        // Steg 3: Skapa annons med prenumerantens uppgifter
        const advertisementData = {
            title: 'Säljer begagnad cykel',
            content: 'Bra skick, nyservad. Passar längd 160-180cm.',
            itemPrice: 2500,
            category: 'fordon',
            publicationDate: new Date().toISOString().split('T')[0],
            
            // Kontaktuppgifter från prenumerant (kan överskridas)
            firstName: subscriber.firstName,
            lastName: subscriber.lastName,
            phoneNumber: subscriber.phoneNumber,
            email: subscriber.email,
            address: subscriber.deliveryAddress,
            postalCode: subscriber.postalCode,
            city: subscriber.city,
            
            // Prenumerantspecifika fält
            advertiserType: 1,
            subscriptionNumber: subscriber.subscriptionNumber
        };

        const result = await this.createAdvertisement(advertisementData);
        console.log('Annons skapad för prenumerant:', result);
    }

    /**
     * Exempel på komplett arbetsflöde för företag
     */
    async companyWorkflowExample() {
        console.log('=== FÖRETAG EXEMPEL ===');
        
        // Steg 1: Beräkna pris (ska vara 40 kr för företag)
        const price = await this.calculateAdvertisementPrice(2);
        console.log(`Annonspris för företag: ${price} kr`);

        // Steg 2: Skapa annons med företagsuppgifter
        const advertisementData = {
            title: 'Bilservice - 20% rabatt i mars',
            content: 'Professionell bilservice med erfarna mekaniker. Boka tid nu och få 20% rabatt på service.',
            itemPrice: 0, // Tjänst, inget fast pris
            category: 'tjanster',
            publicationDate: new Date().toISOString().split('T')[0],
            
            // Företagskontakt
            firstName: 'Johan',
            lastName: 'Andersson',
            phoneNumber: '08-123 456 78',
            email: 'info@bilservice.se',
            address: 'Industrivägen 15',
            postalCode: '12345',
            city: 'Stockholm',
            
            // Företagsspecifika fält
            advertiserType: 2,
            companyName: 'Stockholms Bilservice AB',
            organizationNumber: '556789-1234'
        };

        const result = await this.createAdvertisement(advertisementData);
        console.log('Annons skapad för företag:', result);
    }

    /**
     * Kör alla exempel
     */
    async runExamples() {
        try {
            await this.subscriberWorkflowExample();
            console.log('\n');
            await this.companyWorkflowExample();
            console.log('\n');
            
            // Visa alla annonser
            const allAds = await this.getPublishedAdvertisements();
            console.log('=== ALLA PUBLICERADE ANNONSER ===');
            allAds.forEach(ad => {
                console.log(`${ad.title} - ${ad.advertiserType === 1 ? 'Prenumerant' : 'Företag'} - ${ad.advertisementPrice} kr`);
            });
            
        } catch (error) {
            console.error('Fel i exempel:', error);
        }
    }
}

// Exportera för användning i webbläsare eller Node.js
if (typeof module !== 'undefined' && module.exports) {
    module.exports = AnnouncementSystemIntegration;
}

// Automatiskt exempel när skriptet laddas i webbläsare
if (typeof window !== 'undefined') {
    window.AnnouncementSystemIntegration = AnnouncementSystemIntegration;
    
    // Kör exempel när DOM är laddat
    document.addEventListener('DOMContentLoaded', async () => {
        const integration = new AnnouncementSystemIntegration();
        
        // Lägg till en knapp för att testa integrationen
        const button = document.createElement('button');
        button.textContent = 'Testa API Integration';
        button.style.cssText = 'position: fixed; top: 10px; right: 10px; z-index: 9999; padding: 10px; background: #007bff; color: white; border: none; border-radius: 5px; cursor: pointer;';
        button.onclick = () => integration.runExamples();
        document.body.appendChild(button);
    });
}