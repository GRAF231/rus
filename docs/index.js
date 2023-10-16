YaGames
    .init()
    .then(ysdk => {
        console.log('Yandex SDK initialized');
        window.ysdk = ysdk;
    });
    
var player;

function initPlayer() {
    return ysdk.getPlayer().then(_player => {
        player = _player;

        return player;
    });
}

function auth() {
    initPlayer().then(_player => {
        if (_player.getMode() === 'lite') {
            // Игрок не авторизован.
            ysdk.auth.openAuthDialog().then(() => {
                // Игрок успешно авторизован
                initPlayer().catch(err => {
                    // Ошибка при инициализации объекта Player.
                });
            }).catch(() => {
                    // Игрок не авторизован.
            });
        }
    }).catch(err => {
        // Ошибка при инициализации объекта Player.
    });
}