class AdvertisementSystem {
    constructor() {
        this.apiBaseUrl = 'https://localhost:7043/api';
        this.initializeForm();
        this.checkApiStatus();
    }

    async checkApiStatus() {
        const statusElement = document.getElementById('apiStatus');
        try {
            const response = await fetch(`${this.apiBaseUrl}/subscribers`);
            if (response.ok) {
                statusElement.innerHTML = '<span style="color: green;">Connected</span>';
            } else {
                statusElement.innerHTML = '<span style="color: red;">API Error</span>';
            }
        } catch (error) {
            statusElement.innerHTML = '<span style="color: red;">Not connected</span>';
            console.error('API connection failed:', error);
        }
    }

    initializeForm() {
        // Handle switching between subscriber and company
        const userTypeRadios = document.querySelectorAll('input[name="userType"]');
        userTypeRadios.forEach(radio => {
            radio.addEventListener('change', this.handleUserTypeChange.bind(this));
        });

        // Handle subscription number changes
        const subscriptionInput = document.getElementById('subscriptionNumber');
        subscriptionInput.addEventListener('blur', this.handleSubscriptionNumberChange.bind(this));

        // Handle form submission
        const form = document.getElementById('advertisementForm');
        form.addEventListener('submit', this.handleFormSubmit.bind(this));

        // Set today's date as default for publication
        const publicationDate = document.getElementById('publicationDate');
        publicationDate.value = new Date().toISOString().split('T')[0];
    }

    handleUserTypeChange(event) {
        const isSubscriber = event.target.value === '1';
        const subscriberSection = document.getElementById('subscriber-section');
        const companySection = document.getElementById('company-section');
        const subscriptionNumber = document.getElementById('subscriptionNumber');
        const companyName = document.getElementById('companyName');

        if (isSubscriber) {
            subscriberSection.style.display = 'block';
            companySection.style.display = 'none';
            subscriptionNumber.required = true;
            companyName.required = false;
        } else {
            subscriberSection.style.display = 'none';
            companySection.style.display = 'block';
            subscriptionNumber.required = false;
            companyName.required = true;
            this.clearSubscriberInfo();
        }
    }

    async handleSubscriptionNumberChange(event) {
        const subscriptionNumber = event.target.value.trim().toUpperCase();
        event.target.value = subscriptionNumber;
        
        if (!subscriptionNumber) {
            this.clearSubscriberInfo();
            return;
        }

        try {
            this.showLoading(true);
            
            const response = await fetch(`${this.apiBaseUrl}/subscribers/by-subscription/${subscriptionNumber}`);
            
            if (response.ok) {
                const subscriber = await response.json();
                this.populateSubscriberInfo(subscriber);
                this.showSubscriberFound(true);
            } else if (response.status === 404) {
                this.clearSubscriberInfo();
                this.showSubscriberNotFound();
            } else {
                this.showError('An error occurred while fetching subscriber information');
            }
        } catch (error) {
            console.error('Error fetching subscriber information:', error);
            this.showError('Could not connect to the API. Please check your internet connection.');
        } finally {
            this.showLoading(false);
        }
    }

    populateSubscriberInfo(subscriber) {
        const fields = {
            'firstName': subscriber.firstName,
            'lastName': subscriber.lastName,
            'phoneNumber': subscriber.phoneNumber,
            'email': subscriber.email,
            'address': subscriber.deliveryAddress,
            'postalCode': subscriber.postalCode,
            'city': subscriber.city
        };

        Object.keys(fields).forEach(fieldId => {
            const element = document.getElementById(fieldId);
            if (element && fields[fieldId]) {
                element.value = fields[fieldId];
                element.readOnly = true;
                element.classList.add('auto-filled');
            }
        });
    }

    clearSubscriberInfo() {
        const fieldIds = ['firstName', 'lastName', 'phoneNumber', 'email', 'address', 'postalCode', 'city'];
        
        fieldIds.forEach(fieldId => {
            const element = document.getElementById(fieldId);
            if (element) {
                element.value = '';
                element.readOnly = false;
                element.classList.remove('auto-filled');
            }
        });
        
        this.clearSubscriberMessage();
    }

    clearSubscriberMessage() {
        const messageEl = document.getElementById('subscriberMessage');
        if (messageEl) {
            messageEl.textContent = '';
            messageEl.className = '';
        }
    }

    showSubscriberFound(found) {
        const messageEl = document.getElementById('subscriberMessage');
        if (messageEl && found) {
            messageEl.innerHTML = '<strong>Subscriber found!</strong> Information has been filled in automatically. You can edit it if something is incorrect.';
            messageEl.className = 'message success';
        }
    }

    showSubscriberNotFound() {
        const messageEl = document.getElementById('subscriberMessage');
        if (messageEl) {
            messageEl.innerHTML = 'Subscription number not found. Please fill in the information manually or contact customer service.';
            messageEl.className = 'message warning';
        }
    }

    showError(message) {
        const messageEl = document.getElementById('subscriberMessage');
        if (messageEl) {
            messageEl.innerHTML = `Error: ${message}`;
            messageEl.className = 'message error';
        }
    }

    showLoading(isLoading) {
        const loadingEl = document.getElementById('loading');
        if (loadingEl) {
            loadingEl.style.display = isLoading ? 'block' : 'none';
        }
    }

    async handleFormSubmit(event) {
        event.preventDefault();
        
        const submitBtn = document.getElementById('submitBtn');
        const originalText = submitBtn.textContent;
        
        try {
            submitBtn.disabled = true;
            submitBtn.textContent = 'Sending...';
            
            const formData = this.collectFormData();
            
            const response = await fetch(`${this.apiBaseUrl}/advertisements`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData)
            });

            if (response.ok) {
                const result = await response.json();
                this.showSuccessMessage(result);
                this.resetForm();
            } else {
                const errorText = await response.text();
                this.showSubmissionError(`Error sending ad: ${errorText}`);
            }
        } catch (error) {
            console.error('Submission error:', error);
            this.showSubmissionError('An error occurred while sending the ad. Please try again later.');
        } finally {
            submitBtn.disabled = false;
            submitBtn.textContent = originalText;
        }
    }

    collectFormData() {
        const userType = parseInt(document.querySelector('input[name="userType"]:checked').value);
        
        const formData = {
            title: document.getElementById('title').value,
            content: document.getElementById('content').value,
            itemPrice: parseFloat(document.getElementById('itemPrice').value) || 0,
            category: document.getElementById('category').value,
            publicationDate: document.getElementById('publicationDate').value,
            
            firstName: document.getElementById('firstName').value,
            lastName: document.getElementById('lastName').value,
            phoneNumber: document.getElementById('phoneNumber').value,
            email: document.getElementById('email').value,
            address: document.getElementById('address').value,
            postalCode: document.getElementById('postalCode').value,
            city: document.getElementById('city').value,
            
            advertiserType: userType
        };

        if (userType === 1) { // Subscriber
            formData.subscriptionNumber = document.getElementById('subscriptionNumber').value;
        } else { // Company
            formData.companyName = document.getElementById('companyName').value;
            formData.organizationNumber = document.getElementById('orgNumber').value;
        }

        return formData;
    }

    showSuccessMessage(result) {
        const price = result.advertiserType === 1 ? 'free' : `${result.advertisementPrice} kr`;
        alert(`Advertisement sent!\n\nAd ID: ${result.id}\nAd price: ${price}\nStatus: Published\n\nThank you for your ad!`);
    }

    showSubmissionError(message) {
        alert(`${message}`);
    }

    resetForm() {
        document.getElementById('advertisementForm').reset();
        this.clearSubscriberInfo();
        document.getElementById('publicationDate').value = new Date().toISOString().split('T')[0];
        
        // Reset to subscriber as default
        document.querySelector('input[name="userType"][value="1"]').checked = true;
        this.handleUserTypeChange({ target: { value: '1' } });
    }
}

// Start the system when the page loads
document.addEventListener('DOMContentLoaded', function() {
    new AdvertisementSystem();
});