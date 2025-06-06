# Game_Radionova

## Диаграмма классов
![Диаграмма классов](https://www.mermaidchart.com/play#pako:eNp9VE1vGjEQ_SsWp60SDvSYQw-QBiElCgKqXiJFxh7IKN4xsmeb0ij89hrvBrxeNntZ-703X56x3wfKahjcDJSR3t-i3DpZPpEIn0YHitGSWI2fqMaiSixZOr6zrhTvNXz8rk5o8e2MDtcVs6XR88Sgem0xTmq040gvkbYG5kbuwT1PXkC9gp68SNqC7jNZvdkv9R_tlKeyhDzj4RFcAnOI7oVvFhn_k7ZIICD-Em6OiisHY_tXbNB5jvWBvqzwoCzprmSFJTjh4A9IE9fpwVlrBPqHyjDuYqUJicRCVc4B8Tzn7uUaTJuNUGbulXXNkY_6qe8dx5HMHdaFbMN55WUc98udJAFG7jzo4z6l5doEb3tb8VxScM8ZMOokwMFDHr-xtbu4-Gx9HMvPzhdpt9OhmhHyr1mRQ9EOwegOg9Lgv7rQDrc497LFPVIzDvU98EAa3LWAVBMH-TfSFyN8mtb04nl2ARO3uNmgCuOyT7g4RbPLU3SVeiz0yfy6PXb92TR3I83lOD4L--YzaGJNVVKK3qPnQ535QczC9Ugv3kKStqVw8dfpZh22t5-ramfgEKKGOogPYgo8daiXoWdFHTBVt_KYAoGTDDGfIljntZ-ekeHwR7shN6Ly4HtUzUGlmvMbesGVchCy6FXGTaZKwlxwGJQ6CAcf_wFT8NwK)

---

## Описание классов

### Класс `StartForm`
**Назначение:** начальное окно игры с выбором сложности и режима.

**Основные методы:**
- `button1_Click` — создаёт настройки игры и запускает GameForm.
- `radioButtonSinglePlayer_CheckedChanged, radioButtonTwoPlayer_CheckedChanged` — обработчики переключения режимов (пока не реализованы).

**Зависимости:** создает экземпляры GameSettings и GameForm.

### Класс GameForm
**Назначение:** главное игровое окно с сеткой карточек, таймером и логикой игры.

**Основные методы:**
- `InitUI` — создает панель с информацией (игроки, счёт, таймер).
- `InitGameField` — создаёт карточки и игровую сетку.
- `InitializeTimer` — запускает таймер одиночной игры.
- `InitRevealTimer` — отвечает за скрытие неугаданных карточек.
- `OnPictureClick` — обрабатывает клик по карточке.
- `CheckWin` — проверяет завершение игры и показывает результат.
**Поля:**
- `firstClicked, secondClicked` — выбранные карточки.
- `scorePlayer1, scorePlayer2` — счёт в мультиплеере.
- `timeLabel` — отображает оставшееся время.

**Зависимости:** использует GameSettings, GameEngine.

### Класс GameSettings
Назначение: хранит параметры выбранной игры.

**Свойства:**
- `Difficulty` — уровень сложности ("Лёгкий", "Средний", "Сложный").
- `IsMultiplayer` — флаг мультиплеера.

**Используется в:** StartForm, GameForm, GameEngine.

### Класс GameEngine
**Назначение:** логика генерации поля и карточек.

**Свойства:**
- `Rows, Columns` — размер сетки в зависимости от сложности.
- `Icons` — список путей к изображениям карточек (в парах).

**Методы:**
- `GetGridSize` — определяет размер сетки по уровню.
- `GenerateIcons` — случайным образом выбирает пары изображений.

**Зависимости:** использует GameSettings и файлы из папки Images.
