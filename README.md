# Checkers - multiplayer
## Członkowie:
Krzysztof Szczepański <br />
Adam Kwika <br />
Mikołaj Tomasiczek <br />

## Dlaczego?
Jako grupa chcielibyśmy sprawdzić swoje umiejętnosći i stworzyć pełnoprawną aplikację internetową, pozwalającą graczom grać wspólnie w warcaby.

## Co?
Warcaby - multiplayer

## Jak?
Stworzone za pomocą ASP.NET MVC, .NET 7. Ułatwi nam to komunikację backend-frontend oraz zachowanie dobrych praktyk programowawania i projektowych. Do backendu wykorzystamy EntityFramework oraz SignalR. Do frontendu jQuery, AJAX.

## Backend
### Design
Zastosowany został wzór projektowy "Clean architecture". Zgodnie z jego założeniami baza danych, modele i interfejsy repozytorów znajdują się w warstwie "Domain". W warstwie "Infrastructure" posiadamy implementację komunikacji z bazą danych SQLite. W Warstwie "Application" znajdują się serwisy i kod wykonywany w kontrolerach lub w GameHubie.
### Komunikacja z Frontendem
Standardowa komunikacja w ASP.NET MVC. Po wejściu na stronę kontroler zwraca widok. Z widoku wysyłamy model do kontrolera, który odpowiednio przekierowuje do gry. Na ten moment posiadamy 3 kontrolery: HomeController, JoinGameController i CreateGameController.<br /><br />
- **HomeController** - Zwraca widok główny z listą dostępnych pokoi z bazy danych (jeśli takowe istnieją).<br />
- **CreateGameController** - Odbiera model z widoku zawierający nazwę użytkownika oraz wygenerowany na frontendzie numer pokoju. Tworzy nową grę w bazie danych, przekierowuje na widok GameRoom z modelem Request zawierającym nazwę gracza oraz rodzaj requesta, w tym wypadku "CreateGame". W widoku ustanawiane jest połączenie z SignalR i wysyłane do niego rządanie z modelu ("CreateGame") i rozpoczyna się oczekiwanie na dołączenie drugiego gracza.<br />
- **JoinGameController** - Odbiera model z widoku zawierający nazwę użytkownika oraz podany na frontendzie numer pokoju. Przekierowuje na widok GameRoom z modelem Request zawierającym nazwę gracza oraz rodzaj requesta, w tym wypadku "JoinGame". W widoku ustanawiane jest połączenie z SignalR i wysyłane do niego rządanie z modelu ("JoinGame"). Stamtąd wysyłany jest sygnał do gracza pierwszego z informacją o dołączeniu użytkownika i rozpoczyna się rozgrywka.<br />
<br />
**SignalR** - Do komunikacji między graczami znajdującymi się w grze wykorzystaliśmy SignalR. Narzędzie to pozwala nam tworzyć grupy użytkowników, jako nazwę grupy wykorzystujemy numer pokoju. W jednej grupie może być maksymalnie dwóch graczy. Po wykonaniu przez graczy akcji na frontendzie wysyłany jest sygnał do SignalR poprzez JQuery. Następnie serwer zwraca odpowiedni komunikat, który wywołuje znajdującą się na frontendzie funkcję. Przykładem może być funkcja MakeMove, wysyłająca do SignalR informację o wykonanym ruchu. Następnie SignalR przetwarza informacje, zapisuje ruch do bazy danych i odsyła odpowiednią informację drugiemu graczowi.<br />
## Frontend
Do frontendu wykorzystujemy stworzone przez ASP.NET widoki. Widoki pozwalają nam na używanie HTML, CSS, JQuery, AJAX oraz samego C# w widokach. Stosujemy zasadę "unobstrusive javascript", nie używamy onclick tylko wykorzystujemy id elementów html aby wykonywać na nich akcje w odpowiednich plikach .js. Mamy zainstalowaną paczkę JQuery SignalR, pozwalającą nam na komunikację z backendem. Nie używamy żadnego frameworka ponieważ było by to tak zwanym "overkill". W aplikacji tej wielkości wystarcza nam w zupełności to do czego już mamy dostęp.

