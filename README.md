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

**HomeController/Index** - pobiera dane o grach z game repository, zwraca widok Index.cshtml oraz przekazuje tam model gier
**HomeController/CreateGame(string gameId, string p1Name)** - przyjmuje 2 argumenty, tworzy nową grę w bazie danych, zwraca widok GameRoom.cshtml z modelem RequestViewModel, zawierającym informację o metodzie (tutaj create) oraz o graczu i numerze gry
**HomeController/JoinGame(string gameId, string p2Name)** - przyjmuje 2 argumenty, zwraca widok GameRoom.cshtml z modelem RequestViewModel, zawierającym informację o metodzie (tutaj join) oraz o graczu i numerze gry
**HomeController/GetDashboard()** - pobiera listę zwycięzców gier z bazy danych, zlicza ich ilość wygranych gier i zwraca Json z tą informacją i status 200, kiedy się nie uda zwraca BadRequest ze statusem 400

**SignalR/OnDisconnectedAsync(Exception ex)** - przyjmuje error, kiedy gracz wyjdzie z gry lub utraci połączenie jest wywoływana automatycznie, usuwa gracza z grupy oraz wysyła informację do gracza wciąż połączonego aby po jego stronie wykonała się metoda "PlayerLeft"

**SignalR/SendMessage(string gameId, string message, string messageType)** - przyjmuje 3 argumenty, po id gry znajdowany jest gracz do którego należy wysłać wiadomość, która może mieć jeden z 3 typów: P1, P2, system, zwraca do gracza informację o wywołanie metody "ReciveMessage"

**SignalR/CreateGame(string gameId)** - przyjmuje gameId, dodaje gracza do nowej grupy, tworzy grę w bazie danych, zwraca do wywołującego gracza informację o wywołanie metody "CreateGame"

**SignalR/JoinGame(string gameId, string p2Name)** - przyjmuje 2 argumenty, dołącza gracza do gry i aktualizuje bazę danych, dodaje gracza do grupy gdzie oczekuje na niego gracz 1 jeżeli takowa istnieje, jeżeli nie to odsyła informację o wywołanie metody "AbandonedGame". Jeżeli istnieje, to wysyła do użytkownika wywołującego informację o wywołanie metody "YouJoined" a do drugiego "TableJoined"

**SignalR/MakeMove(
            int previousCheckerRow, int previousCheckerColumn, 
            int nextCheckerRow, int nextCheckerColumn, 
            int checkerToDeleteRow, int checkerToDeleteColumn,
            string gameId, string currentPlayer))** - przyjmuje 8 argumentów, tworzy ruch do bazy danych w formacie previousCheckerColumn-previousCheckerRow_nextCheckerColumn-nextCheckerRow. Odsyła do użytkownika wywołującego informację o wywołanie metody "YouMoved" a do drugiego "EnemyMoved" z informacją o ruchu oraz opcjonalnym zbitym pionkiem

**SignalR/YouWin(string gameId, string name)** - przyhmuje 2 argumenty, aktualizuje wygranego w bazie danych, odsyła do wywołującego użytkownika informację o wywołanie metody "HandleVictory" a do drugiego "HandleDefeat"

**SignalR/YouLost(string gameId, string name)** - wykonuje odwrotność tego co YouWin
## Frontend
Do frontendu wykorzystujemy stworzone przez ASP.NET widoki. Widoki pozwalają nam na używanie HTML, CSS, JQuery, AJAX oraz samego C# w widokach. Stosujemy zasadę "unobstrusive javascript", nie używamy onclick tylko wykorzystujemy id elementów html aby wykonywać na nich akcje w odpowiednich plikach .js. Mamy zainstalowaną paczkę JQuery SignalR, pozwalającą nam na komunikację z backendem. Nie używamy żadnego frameworka ponieważ było by to tak zwanym "overkill". W aplikacji tej wielkości wystarcza nam w zupełności to do czego już mamy dostęp.
### Szczegółowy opis fucnkji JQuery oraz SignalR
1. Na frontendzie odbywa się generacja nowego id pokoju do którego dołączy lub który stworzy użytkownik.
2. Wykorzystując bootstrapowe modal otwieramy okna dialogowe do rpzesyłania na frontend takich informacji jak nazwa gracza.
3. Połączenie SignalR ustalane jest na frontendzie

**site.js/createNewRoom()** - wywołuje metodę generateNumber() i zwraca go

**site.js/generateNumber()** - generuje losowy 6 cyfrowy numer i zwraca go

**indexSite.js/** - po załadowaniu dokumentu wysyła AJAX do endpointu /Home/GetDashboard o typie GET. Generuje listę wygranych graczy

**gameRoom.js/** - po załadowaniu wywołuje 3 metody: CheckerOnClick();CellOnClick();HandleMessage();

**gameRoom.js/CheckWinConditions()** - liczy ilość pionków każdego gracza i sprawdza czy któryś z nich ma 0. Jeżeli tak to wywołuje metodę YouWin, jeżeli nie to YouLost

**gameRoom.js/HandleMessage()** - odpowiada za obsłużenie przycisku wysyłania wiadomości, po kliknięciu wywołuje metodę SendMessage

**gameRoom.js/CheckerOnClick** - odpowiada za obsłużenie kliknięcia na pionek, zaznacza pionek jako aktywny poprzez nadanie mu odpowiedniej klasy i oblicza gdzie pionek może wykonać swoje ruchy pooprzez wywołanie metody EvaluateCheckerSpots

**gameRoom.js/CellOnClick** - odpowiada za obsłużenie kliknięcia na pole, jeżeli było ono zaznaczone jako dostępne to przesuwa tam aktywny pion, jeżeli po drodze przeskoczony został pion przeciwnika to jest on usuwany, jeżeli pion dotrze do krańca planszy to zamienia go w "damkę"

**gameRoom.js/EvaluateCheckerSpots** - oblicza możliwość wolnych pól, sprawdza czy pola nie są na krańcach planszy oraz czy po drodze nie ma możliwości bicia poprzez wykonanie metody CanJumpOver

**gameRoom.js/CanJumpOver** - sprawdza czy można przeskoczyć pionek znajdujący się na jednym z potencjalnych pól do ruchu

**gameRoom.js/UpdateTable(newMove, checkerToDeletePos)** - przyjmuje 2 argumenty, odświeża planszę po wykonaniu ruchu przeciwnika, zmieniając pozycję pionków oraz usuwając zbite pionki

**gameHub.js/** - po załadowaniu dokumentu nawiązuje połączenie z serwerem poprzez SignalR, opowiada za odpowiedzi z serwera i wysyłanie do niego żądań
## Struktura bazy danych
### Szczególowy opis tabel
** Game **:
- Id [Key] (int)
- P1 (string)
- P2 (string)
- Winner (string)
Przykład: 123456, "Gracz 1", "Gracz 2", "Gracz 1"
** Moves **:
- Id [Key] (int)
- GameId (int)
- P1_Moves (string)
- P2_Moves (string)
Przykład: 1, 123456, "1-1_2-3;2-3_3-5;", "1-1_2-3;2-3_3-5;"
