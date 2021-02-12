# WILD BALL
Do wykonania tej gry posłużyłem się silnikiem do tworzenia gier **Unity 2019.4.19f1 Personal**
oraz językiem **C#** do napisania skryptów, pojawił się tam również **ShaderLab** z assetów, **ASP.NET** oraz **HLSL**.

W programie Unity stworzyłem cztery klasy Scen, które przedstawiają konkretne sceny z gry.

## SplashScreen
Menu główne, w którym można wybrać dwie opcje. **START** bądź **QUIT**, w przypadku wciśnięcia przycisku **START** wczyta się scena o nazwie **Level01**.
Poza tym widać zapisaną na zielono nazwę gry **WILD BALL**.
Gdyby użytkownik zdecydował się wcisnąć przycisk **QUIT**, wówczas okno aplikacji zostałoby wyłączone.
Elementy w Scenie zostały stworzone za pomocą zestawu UI z programu Unity. Oraz systemu wydarzeń, również z programu Unity.
W przypadku gdy użytkownik najedzie na jakiś przycisk, zacznie on się rozjaśniać, gdy zostanie wciśnięty, również rozjaśni się jeszcze bardziej.
Do obsłużenia głównego menu stworzyłem specjalny skrypt o nazwie `MainMenu.cs`, który zawiera wiele użytecznych metod oraz funkcji.

`PlayGame()` wywołuje się w momencie, kiedy użytkownik wciśnie przycisk **START**.
```    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +2);
    }
```
Powyższy fragment kodu służy głównie do tego, aby przesunąć aktualną scenę o dwie do przodu, czyli do sceny o nazwie **Level01**.
Pomiędzy nie wepchałem również jedną scenę, którą omówię później.

`QuitGame()` jest kolejną ważną funkcją, o której warto wspomnieć. Wywołuje się, gdy użytkownik wciśnie przycisk **QUIT**.
```    public void QuitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();

    }
```
Po powyższym fragmencie kodu widać, że jedynym przeznaczeniem funkcji jest wyjście z aplikacji,
oraz pokazanie w Debuggerze (gdy takie testowanie jest niemożliwe), że funkcja się wykonuje.

## Retry
Kolejna istotna scena, dzięki której użytkownik widzi, w którym momencie jego podejście do poziomu zostało zakończone porażką.
W tym momencie pokazują mu się dwa przyciski, które może wcisnąć. Jeden to **RETRY** a drugi to **QUIT** za pomocą **RETRY** może powtórzyć podejście do poziomu,
natomiast dzięki przyciskowi **GO BACK** może powrócić do sceny **SplashScreen** czyli inaczej głównego menu, gdzie może rozpocząć grę, lub wyjść z programu.
W tej scenie użyłem głównie obsługi wydarzeń z programu Unity, która wykrywa interakcje myszy użytkownika. Prócz tego typowo rzut kamery użytkownika.
Główną wisienką na torcie jest tutaj całe tło, a w nim dwa przyciski. Jeden o nazwie **RETRY**, a drugi o nazwie **GO BACK**.

`RetryGame()` funkcja polega głównie na tym, że w zależności od aktywnej sceny przechodzi o jedną do przodu. W tym wypadku przechodzi do sceny **Level01**,
na której toczy się rozgrywka (a raczej kula :-D).
```
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
```

`GoBackToMenuFromRetry()` inaczej funkcja, która przenosi użytkownika do menu z poziomu sceny **Retry** do sceny **SplashScreen**.
```    public void GoBackToMenuFromRetry()
    {
        Debug.Log("Go back to Menu from Retry");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
```
Po powyższym fragmencie kodu widać, że tutaj również dodałem Log Debuggera, gdyż utknąłem przy tym przykładzie na chwilę. Jak się potem okazało, zapomniałem
wtedy dodać obsługę wydarzeń w Unity. Ostatnia linia kodu działa na takiej zasadzie, że przenosi nas scenę wstecz, czyli do sceny **SplashScreen**.

## Level01
Jest to według mnie najbardziej skomplikowana scena, wokół której skupiona jest cały ten program. Gdy użytkownik wejdzie w tę scenę, jego oczom ukaże się zielona
kula i plansza, a obiekt przypisany jako **Player** będzie sterowany przez niego za pomocą klawiszy **A** oraz **D**. Kula będzie się toczyć sama i będzie również
podskakwiać przy tym. Prawa fizyki również zostały zaimplementowane z programu Unity i warunkują one na to, jak zachowa się przeszkoda oraz piłka w zderzeniu ze sobą.
Stworzyłem kilka osobnych tagów obiektów, z czego można wyróżnić 3 główne: **Obstacle**, **Player**, **Finish**. Każdy tag otrzymał specjalny materiał. Obiekt
**Player** otrzymał specjalny skrypt stworzony przeze mnie, o nazwie **Player Movement**.

```
public class PlayerMovement : MonoBehaviour {

    public Rigidbody rb;

    public float fowardForce = 2000f;
    public float sidewaysForce = 500f;

    // Update is called once per frame
    void FixedUpdate() {
        
        // Add a forward force
        rb.AddForce(0, 0, fowardForce * Time.deltaTime);
        
        if ( Input.GetKey("d") )
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0);
            
        }

        if ( Input.GetKey("a") )
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0);
        }

        if (rb.position.y < -1f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }

    }
}
```
Więc tak, Rigidbody jest komponentem wbudowanym w silnik Unity. Zrobiłem w tamtym momencie odwołanie **Rb**, aby można było dodać obiekt **Player** aby został
przypisany do tego skryptu. Nadpisałem również zmienne takie jak **fowardForce** (umknęło mi tutaj **r**) oraz **sidewaysForce**. Jest to prędkość, z jaką porusza się piłka zarówno do przodu, jak i na boki. **fowardForce** jest odpowiedzialne za poruszanie się obiektu na osi Z, natomiast **sidewaysForce** jest odpowiedzialne za
poruszanie się na osi X. Również widać tutaj zaimplementowane z silnika Unity rozwiązanie na sterowanie kulą. W przypadku gdy użytkownik wprowadzi klawisz **d**,
wówczas kula zacznie przesuwać się w prawo (czyli o dodatnią wartość X), w przypadku gdy użytkownik wprowadzi klawisz **a**, wówczas kula zacznie przesuwać się w
lewo (czyli o ujemną wartość X). Jest tutaj również wyłapywanie wypadnięcia poza mapę. Bardziej łopatologicznie można stwierdzić, że w momencie, kiedy pozycja gracza
osiągnie mniejszą wartość od **-1f** na płaszczyźnie Y, wówczas wywoła się polecenie 'FindObjectOfType<GameManager&gt;().EndGame();', które zostało zaimplementowane z
silnika Unity.

Wyświetlany aktualny **Score** to jest po prostu wartość przesunięcia na osi obiektu **Player**. Do sceny został również dodany system obsługi wydarzeń.
```
public class Score : MonoBehaviour
{
    public Transform player;
    public Text scoreText;

    void Update() {
        scoreText.text = player.position.z.ToString("0");
    }
}
```
Jak widać po powyższym kodzie, pobiera on zmieniane koordynaty gracza, oraz tworzy zmienną tekstową dla `scoreText` a `scoreText` cały czas się aktualizuje,
wówczas gdy koordynat `z` zmienia się w obiekcie `player`, czyli inaczej w obiekcie gracza, a jeszcze inaczej w kuli, którą sterujemy. Ważna tutaj była
adnotacja `using UnityEngine.UI;`, tak jak zresztą w reszcie skryptów, aby można było korzystać z dobrodziejstw silnika Unity w moich skryptach.

Teraz pasuje omówić trochę bardziej system kolizji z obiektami.

```
public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;

    void OnCollisionEnter (Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacle") {

            movement.enabled = false;
            FindObjectOfType<GameManager>().EndGame();
            
            

            

        }

            if (collisionInfo.collider.tag == "Finish") {

            movement.enabled = false;
            FindObjectOfType<GameManager>().FinishGame();

            

        }
    }
}
```

No więc tak, została tutaj pobrana zmienna z innego skryptu (`PlayerMovement.cs`), oraz użyta funkcja z silnika Unity o nazwie `OnCollisionEnter`, która pobiera dwa 
parametry `Collision` oraz `collisionInfo`. Ja dodałem dwa przypadki, w których program sprawdza kolizję dla dwóch tagów obiektów `Obstacle` oraz `Finish`. `Obstacle` 
jest odpowiedzialny za to, aby zakończyć rozgrywkę i wywołać funkcję `EndGame()`, natomiast `Finish` jest odpowiedzialny za to, aby wywołać funkcję `FinishGame()`, 
aby ukończyć grę.

W grze zostało również dodane dynamiczne oświetlenie, dzięki któremu obiekty rzucają cień, a nawet posiadają na sobie w miejscach, gdzie światło nie dociera.

```
public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
        
    }
}
```

Nie wspominałem nic o powyższym snippecie kodu, a jest on również bardzo ważny. Aktualizuje się cały czas, dzięki czemu kamera porusza się za kulką. Funkcja pobiera
koordynaty z obiektu 'player', dzięki czemu nie kręci się wokół kuli, a wyłącznie porusza się za nią o konkretny 'offset'. Gdy stworzymy taki skrypt,
musimy go podpiąć pod 'Main Camera', aby funkcjonował, a następnie poprawnie go ustawić. Po poprawnym podpięciu ustawiłem offsety, z racji, iż jest to 'Vector3', to
poprosi nas o podanie trzech zmiennych 'X', 'Y', 'Z'. Uznałem, że zmienianie 'X' nie ma sensu, ponieważ użytkownik nie musi patrzeć z boku, jak piłka skacze, miałoby
to sens w przypadku tworzenia gier wyścigowych, gdzie gracz mógłby mieć kamerę przy felgach, jednak 'X' nie grał tutaj roli. 'Y' ustawiłem na '1', aby gracz był
w stanie widzieć coś poza odbijającą się kulą, a z racji, że obiekty były w oddali, to miało to sens, aby widzieć więcej terenu, inaczej jedyne co użytkownik by
widział, to odbijającą się kulę. Parametr 'Z' ustawiłem na '-5', bo gdybym ustawił go na dodatni, to kamera byłaby przed kulą, a myślę, że użytkownik wolałby widzieć, jak kula się odbija i czy właśnie nie wpada na przeszkodę, niż gdyby miał nie wiedzieć co dzieje się z jego kulą.

# FinalScreen
Określiłbym to jako scenę finałową, znajduje się tam komunikat **CONGRATULATIONS! YOU WON!**, więc gracz zostaje pochwalony, co mu wynagradza trud poświęcony na
przejście jednego poziomu tej gry. Wyskakuje mu również przycisk, który jest niebieski, a na nim napisane jest **MENU**. Nie ma tutaj niczego szczególnego. Tak jak w
reszcie dodałem rzut kamery, oświetlenie, którego zresztą nie widać, system obsługi wydarzeń.

Znajduje się tutaj jeden fragment kodu podpięty pod przycisk, który warto omówić.

```
    public void GoBackToMenu()
    {
        Debug.Log("BACK");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -3);
    }
```

Powyższy fragment kodu wysyła logi do Debuggera, wskazujące na to, że gracz chce wcisnąć przycisk i się wycofać. Było to o tyle przydatne, że wiedziałem, kiedy
przycisk był wciskany, działał, a 'SceneManager' nie rozumiał, co chcę mu przekazać. Nie miałem tutaj na początku również systemu obsługi wydarzeń, lecz potem
program został odkryty. Kolejna linia kodu cofa gracza o trzy sceny do tyłu, czyli na scenę **SplashScreen**, czyli nie wyświetla żadnego komunikatu, że gracz
przegrał, pokazuje tylko ekran startowy i możliwość rozpoczęcia nowej gry, bądź wyjścia z programu.

I to by było na tyle, dziękuję za doczytanie się aż do tego momentu i życzę miłej gry (bądź exploitu kodu źródłowego). :-)
