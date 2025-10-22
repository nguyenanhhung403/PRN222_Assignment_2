// SignalR Connection Manager
const SignalRManager = {
    connections: {},

    // Initialize a SignalR connection
    initConnection: function(hubName, hubUrl) {
        if (this.connections[hubName]) {
            console.log(`Connection ${hubName} already exists`);
            return this.connections[hubName];
        }

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(hubUrl)
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        // Handle reconnection events
        connection.onreconnecting(error => {
            console.log(`${hubName} connection lost. Reconnecting...`, error);
            this.showConnectionStatus(`Reconnecting to ${hubName}...`, 'warning');
        });

        connection.onreconnected(connectionId => {
            console.log(`${hubName} reconnected. Connection ID: ${connectionId}`);
            this.showConnectionStatus(`Reconnected to ${hubName}`, 'success');
        });

        connection.onclose(error => {
            console.log(`${hubName} connection closed`, error);
            this.showConnectionStatus(`Connection to ${hubName} closed`, 'error');
        });

        this.connections[hubName] = connection;
        return connection;
    },

    // Start a connection
    startConnection: async function(hubName) {
        const connection = this.connections[hubName];
        if (!connection) {
            console.error(`Connection ${hubName} not initialized`);
            return;
        }

        try {
            await connection.start();
            console.log(`${hubName} connected successfully`);
            return true;
        } catch (error) {
            console.error(`Error connecting to ${hubName}:`, error);
            setTimeout(() => this.startConnection(hubName), 5000); // Retry after 5 seconds
            return false;
        }
    },

    // Stop a connection
    stopConnection: async function(hubName) {
        const connection = this.connections[hubName];
        if (connection) {
            await connection.stop();
            delete this.connections[hubName];
        }
    },

    // Get a connection
    getConnection: function(hubName) {
        return this.connections[hubName];
    },

    // Show connection status (you can customize this)
    showConnectionStatus: function(message, type) {
        console.log(`[${type.toUpperCase()}] ${message}`);
        // You can add UI notification here if needed
    }
};

// Make it globally available
window.SignalRManager = SignalRManager;
