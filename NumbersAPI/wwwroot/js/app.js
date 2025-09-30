class NumbersApp {
    constructor() {
        this.apiBaseUrl = '/api/numbers';
        this.initializeEventListeners();
        this.loadInitialData();
    }

    initializeEventListeners() {
        document.getElementById('addNumberBtn').addEventListener('click', () => this.addNumber());
        document.getElementById('clearNumbersBtn').addEventListener('click', () => this.clearNumbers());
        document.getElementById('sumNumbersBtn').addEventListener('click', () => this.sumNumbers());
    }

    async loadInitialData() {
        try {
            const response = await this.fetchData('GET', '');
            this.updateUI(response);
        } catch (error) {
            console.error('Error loading initial data:', error);
            this.showError('Failed to load initial data');
        }
    }

    async addNumber() {
        this.setLoading(true);
        try {
            const response = await this.fetchData('POST', '/add');
            this.updateUI(response);
        } catch (error) {
            console.error('Error adding number:', error);
            this.showError('Failed to add number');
        } finally {
            this.setLoading(false);
        }
    }

    async clearNumbers() {
        this.setLoading(true);
        try {
            const response = await this.fetchData('POST', '/clear');
            this.updateUI(response);
        } catch (error) {
            console.error('Error clearing numbers:', error);
            this.showError('Failed to clear numbers');
        } finally {
            this.setLoading(false);
        }
    }

    async sumNumbers() {
        this.setLoading(true);
        try {
            const response = await this.fetchData('GET', '/sum');
            this.updateUI(response);
        } catch (error) {
            console.error('Error calculating sum:', error);
            this.showError('Failed to calculate sum');
        } finally {
            this.setLoading(false);
        }
    }

    async fetchData(method, endpoint) {
        const response = await fetch(`${this.apiBaseUrl}${endpoint}`, {
            method: method,
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            const errorData = await response.json().catch(() => ({}));
            throw new Error(errorData.error || `HTTP error! status: ${response.status}`);
        }

        return await response.json();
    }

    updateUI(data) {
        this.updateNumbersList(data.numbers);
        this.updateCount(data.count);
        this.updateSum(data.sum);
    }

    updateNumbersList(numbers) {
        const numbersList = document.getElementById('numbersList');
        
        if (numbers.length === 0) {
            numbersList.innerHTML = '<p class="empty-message">No numbers added yet</p>';
            return;
        }

        const numbersHTML = numbers.map(number => `
            <div class="number-item">
                <span class="number-value">${number.value}</span>
                <span class="number-id">ID: ${number.id}</span>
            </div>
        `).join('');

        numbersList.innerHTML = numbersHTML;
    }

    updateCount(count) {
        document.getElementById('numbersCount').textContent = count;
    }

    updateSum(sum) {
        document.getElementById('numbersSum').textContent = sum;
    }

    setLoading(loading) {
        const buttons = document.querySelectorAll('.btn');
        const container = document.querySelector('.container');
        
        if (loading) {
            container.classList.add('loading');
            buttons.forEach(btn => btn.disabled = true);
        } else {
            container.classList.remove('loading');
            buttons.forEach(btn => btn.disabled = false);
        }
    }

    showError(message) {
        // Simple error display - in a production app, you might want a more sophisticated notification system
        alert(`Error: ${message}`);
    }
}

// Initialize the app when the DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new NumbersApp();
});
