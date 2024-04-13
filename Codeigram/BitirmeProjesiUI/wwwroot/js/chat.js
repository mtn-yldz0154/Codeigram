// chat.js

var connection = new signalR.HubConnectionBuilder()
    .withUrl('/sohbetHub')
    .build();

connection.start().then(function () {
    console.log('SignalR Bağlantısı Kuruldu!');
}).catch(function (err) {
    console.error(err.toString());
});

function showChatBox(personId, imgSrc, username, userId) {
    activePersonId = personId;

    chatContent.innerHTML = `
        <div class="conversation-head">
            <figure></figure>
            <img width="60" height="40" style="border-radius: 100%" src="${imgSrc}" alt="">
            <span>${username} <i>online</i></span>
        </div>
        <ul class="chatting-area" id="chattingArea"></ul>
    `;

    updateChatBox(personId, userId, imgSrc);  // Move this line here
}


var activePersonId;
var toToken = "";
var userToken = "";


function sendMessage() {
    var messageInput = document.getElementById('messageInput');
    var messageContent = messageInput.value.trim();

    if (messageContent !== '') {
        connection.invoke('SendMessage', toToken, userToken, messageContent)
            .then(function () {
                // Gönderilen mesajı hemen ekleyerek güncelle
                appendMessage(userToken, messageContent);
            })
            .catch(function (err) {
                console.error('SendMessage hatası:', err.toString());
            });

        // Input değerini temizle
        messageInput.value = '';
    } else {
        console.error("Kişi seçilmedi veya mesaj içeriği boş.");
    }
}

// Gönderilen mesajı ekleme fonksiyonu
function appendMessage(userId, message) {
    console.log("buraya girdi", userId);
    var messageHtml = <li class="me"><p>${message}</p></li>;
    var chattingArea = document.getElementById('chattingArea');

    if (chattingArea) {
        chattingArea.innerHTML += messageHtml;
        scrollToBottom(chattingArea);
    } else {
        console.error("ID'si 'chattingArea' olan element bulunamadı.");
    }
}

function updateChatBox(personId, userId, imgSrc) {
    var chattingArea = document.getElementById('chattingArea');

    if (!chattingArea) {
        console.error("ID'si 'chattingArea' olan element bulunamadı.");
        return;
    }

    fetch(/Message/GetMessage ? id = ${ personId })
        .then(response => response.json())
        .then(messages => {
            // Mevcut içeriğe yeni mesajları ekleyin
            messages.forEach(message => {
                var messageHtml = '';
                if (message.senderToken == userId) {
                    messageHtml = `
                        <li class="me">
                            <p>${message.messages}</p>
                        </li>`;
                } else {
                    messageHtml = `
                        <li class="you">
                            <figure><img width="60" height="40" style="border-radius: 100%" src="${imgSrc}" alt=""></figure>
                            <p>${message.messages}</p>
                        </li>`;
                }

                chattingArea.innerHTML += messageHtml;
            });

            // Sohbet alanını en alta kaydır
            enAltaKaydir(chattingArea);
        })
        .catch(error => {
            console.error('Fetch isteği hatası:', error);
        });
}

// Sohbet alanını en alta kaydıran fonksiyon
function enAltaKaydir(element) {
    element.scrollTop = element.scrollHeight;
}


function scrollToBottom(element) {
    // Scroll the element to the bottom
    element.scrollTop = element.scrollHeight;
}