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
- **JoinGameController** - Odbiera model z widoku zawierający nazwę użytkownika oraz podany na frontendzie numer pokoju. Przekierowuje na widok GameRoom z modelem Request zawierającym nazwę gracza oraz rodzaj requesta, w tym wypadku "JoinGame". W widoku ustanawiane jest połączenie z SignalR i wysyłane do niego rządanie z modelu ("JoinGame"). Stamtąd wysyłany jest sygnał do gracza pierwszego z informacją o dołączeniu użytkownika i rozpoczyna się rozgrywka.<br /><br />
**SignalR** - Do komunikacji między graczami znajdującymi się w grze wykorzystaliśmy SignalR. Narzędzie to pozwala nam tworzyć grupy użytkowników, jako nazwę grupy wykorzystujemy numer pokoju. W jednej grupie może być maksymalnie dwóch graczy. Po wykonaniu przez graczy akcji na frontendzie wysyłany jest sygnał do SignalR poprzez JQuery. Następnie serwer zwraca odpowiedni komunikat, który wywołuje znajdującą się na frontendzie funkcję. Przykładem może być funkcja MakeMove, wysyłająca do SignalR informację o wykonanym ruchu. Następnie SignalR przetwarza informacje, zapisuje ruch do bazy danych i odsyła odpowiednią informację drugiemu graczowi.<br />
### Szczegółowy opis komunikacji z bazą danych oraz metod SignalR
1. Kiedy użytkownik wejdzie do game roomu, poprzez JoinRoom albo CreateRoom endpoint z kontrolera następuje ustanowienie połączenia z serwerem SignalR.
2. Użytkonik wykonując akcje wysyła za każdym razem polecenie do odpowiedniej metody serwera, np. MakeMove.
3. Serwer odpowiada nazwą metody jaka ma być wykonana po stronie użytkownika plus opcjonalne parametry. Np. adres komórki ruchu pionka.
4. Przy pierwszym połączeniu z serwerem z endpointa CreateRoom użytwkonik dodawany jest do nowej dwuosobowej grupy o nazwie id gry na serwerze.
5. Kiedy użytkownik dołączy do gry z endpointa JoinRoom, jest on dodawany do grupy o id gry do której dołącza.
6. Grupy są dwuosobowe, po dołączeniu drugiego gracza nikt więcej nie może tego zrobić.
7. Kiedy jeden z graczy wyjdzie, wykonywana jest odpowiednia metoda OnDisconnect, która zapewnia graczowi wciąż połączonemu do serwera automatycze zwycięstwo.
## Frontend
Do frontendu wykorzystujemy stworzone przez ASP.NET widoki. Widoki pozwalają nam na używanie HTML, CSS, JQuery, AJAX oraz samego C# w widokach. Stosujemy zasadę "unobstrusive javascript", nie używamy onclick tylko wykorzystujemy id elementów html aby wykonywać na nich akcje w odpowiednich plikach .js. Mamy zainstalowaną paczkę JQuery SignalR, pozwalającą nam na komunikację z backendem. Nie używamy żadnego frameworka ponieważ było by to tak zwanym "overkill". W aplikacji tej wielkości wystarcza nam w zupełności to do czego już mamy dostęp.
### Szczegółowy opis fucnkji JQuery oraz SignalR
1. Na frontendzie odbywa się generacja nowego id pokoju do którego dołączy lub który stworzy użytkownik.
2. Wykorzystując bootstrapowe modal otwieramy okna dialogowe do rpzesyłania na frontend takich informacji jak nazwa gracza.
3. Połączenie SignalR ustalane jest na frontendzie.
## Struktura bazy danych
### Szczególowy opis tabel
** Game **:
- Id [Key](int)
- P1 (string)
- P2 (string)
- Winner (string)
Przykład: 123456, "Gracz 1", "Gracz 2", "Gracz 1"
** Moves **:
- Id [Key](int)
- GameId (int)
- P1_Moves (string)
- P2_Moves (string)
Przykład: 1, 123456, "1-1_2-3;2-3_3-5;", "1-1_2-3;2-3_3-5;"
