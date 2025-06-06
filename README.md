# Game_Radionova

## Диаграмма классов
![Диаграмма классов](https://www.mermaidchart.com/app/projects/6ccb976d-2170-4695-80ad-c7016a1a6312/diagrams/8a2dbc46-e2b3-469a-b9a6-0ddfb631d54a/share/invite/eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJkb2N1bWVudElEIjoiOGEyZGJjNDYtZTJiMy00NjlhLWI5YTYtMGRkZmI2MzFkNTRhIiwiYWNjZXNzIjoiVmlldyIsImlhdCI6MTc0OTIxNDExMH0.p5YobdkOk3bkJJhDJoNXZC5gD4JDwJwsqRTIAdsOzoI)

---

## Описание классов

### Класс `StartForm`
Назначение: начальное окно игры с выбором сложности и режима.

**Основные методы:**
- `button1_Click` — создаёт настройки игры и запускает GameForm.
- radioButtonSinglePlayer_CheckedChanged, radioButtonTwoPlayer_CheckedChanged — обработчики переключения режимов (пока не реализованы).

Зависимости: создает экземпляры GameSettings и GameForm.

### Класс GameForm
Назначение: главное игровое окно с сеткой карточек, таймером и логикой игры.

**Основные методы:**
- InitUI — создает панель с информацией (игроки, счёт, таймер).
- InitGameField — создаёт карточки и игровую сетку.
- InitializeTimer — запускает таймер одиночной игры.
- InitRevealTimer — отвечает за скрытие неугаданных карточек.
- OnPictureClick — обрабатывает клик по карточке.
- CheckWin — проверяет завершение игры и показывает результат.
Поля:
- firstClicked, secondClicked — выбранные карточки.
- scorePlayer1, scorePlayer2 — счёт в мультиплеере.
- timeLabel — отображает оставшееся время.

Зависимости: использует GameSettings, GameEngine.

### Класс GameSettings
Назначение: хранит параметры выбранной игры.

Свойства:
- Difficulty — уровень сложности ("Лёгкий", "Средний", "Сложный").
- IsMultiplayer — флаг мультиплеера.

Используется в: StartForm, GameForm, GameEngine.

### Класс GameEngine
Назначение: логика генерации поля и карточек.

Свойства:
- Rows, Columns — размер сетки в зависимости от сложности.
- Icons — список путей к изображениям карточек (в парах).

Методы:
- GetGridSize — определяет размер сетки по уровню.
- GenerateIcons — случайным образом выбирает пары изображений.

Зависимости: использует GameSettings и файлы из папки Images/.
