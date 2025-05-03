
const notificationButton = document.querySelector("#notification-dropdown-button");

notificationButton?.addEventListener('click', () => {
    const dot = notificationButton.querySelector(".dot.dot-red");
    if (dot) {
        dot.remove();
    }
});
    

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

    connection.on("ReceiveNotification", function (notification) {
    const notifications = document.querySelector(".notifications");

    const item = document.createElement("div");
    item.className = "notification-item";
    item.setAttribute("data-id", notification.id);
    item.innerHTML = `
    <div class="notification" data-notification-type="user">
        <div class="notification-image">
            <img src="${notification.icon}" alt="">
        </div>
        <span class="message">${notification.message}</span>
        <span class="time" data-created="${new Date(notification.created).toISOString()}">
            ${notification.created}
        </span>
        <button class="btn-remove btn-remove-notification" onclick="dismissNotification('${notification.id}')"></button>
    </div>
    `;

    notifications.insertBefore(item, notifications.firstChild);

    updateRelativeTimes();
    updateNotificationCount();
});

    connection.start();

    async function dismissNotification(notificationId) {
        console.log("Trying to dismiss", notificationId);
        try {
            const res = await fetch(`/api/notifications/dismiss/${notificationId}`, { method: 'POST' });
            console.log("Status:", res.status);
            if (res.ok) {
                removeNotification(notificationId);
            } else {
                console.error('Error removing notification');
            }
        } catch (error) {
            console.error('Error removing notification: ', error);
        }
    }

    function removeNotification(notificationId) {
        const element = document.querySelector(`.notification-item[data-id="${notificationId}"]`);
        if (element) {
            element.remove();
            updateNotificationCount();
        }
    }

    function updateNotificationCount() {
        const notifications = document.querySelector(".notifications");
        const notificationNumber = document.querySelector(".notification-number");
        const notificationDropdownButton = document.querySelector("#notification-dropdown-button");

        const count = notifications.querySelectorAll(".notification-item").length;

        if (notificationNumber) {
            notificationNumber.textContent = count;
        }

        let dot = notificationDropdownButton.querySelector(".dot.dot-red");

        if (count > 0 && !dot) {
            dot = document.createElement("i");
            dot.className = "dot dot-red fa-solid fa-circle";
            const bellIcon = notificationDropdownButton.querySelector(".fa-bell");
            notificationDropdownButton.insertBefore(dot, bellIcon);
        }

        if (count === 0 && dot) {
            dot.remove();
        }
    }