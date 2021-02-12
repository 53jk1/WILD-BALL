# WILD BALL
Aby wykonać tę grę posłużyłem się silnikiem do tworzenia gier **Unity 2019.4.19f1 Personal**
oraz językiem **C#** do napisania skryptów, pojawił się tam również **ShaderLab** z assetów, **ASP.NET** oraz **HLSL**.

W programie Unity stworzyłem cztery klasy Scen, które przedstawiają konkretne sceny z gry.

## SplashScreen
Menu główne, w którym można wybrać dwie opcje. **START** bądź **QUIT**, w przypadku wciśnięcia przycisku **START** wczyta się scena o nazwie **Level01**.
Poza tym widać zapisaną na zielono nazwę gry **WILD BALL**.
Gdyby użytkownik zdecydował się wcisnąć przycisk **QUIT**, wówczas okno aplikacji zostałoby wyłączone.
Elementy w Scenie zostały stworzone za pomocą zestawu UI z programu Unity. Oraz systemu wydarzeń, również z programu Unity.
W przypadku gdy użytkownik najedzie na jakiś przycisk zacznie on się rozjaśniać, gdy zostanie wciśnięty również rozjaśni się jeszcze bardziej.
Do obsłużenia głównego menu stworzyłem specjalny skrypt o nazwie `MainMenu.cs`, któy zawiera wiele użytecznych metod oraz funkcji.

`PlayGame()` wywołuje się w momencie, kiedy użytkownik wciśnie przycisk **START**.
```    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +2);
    }
```
Powyższy fragment kodu służy głównie do tego, aby przesunąć aktualną scenę o dwie do przodu, czyli do sceny o nazwie **Level01**.
Pomiędzy nie wepchałem również jedną scenę, którą omówię bardziej później.

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

`RetryGame()` funckja polega głównie na tym, że w zależności od aktywnej sceny przechodzi o jedną do przodu. W tym wypadku przechodzi do sceny **Level01**,
na której toczy się rozgrywka (a raczej kula :-D).
```
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
```

`GoBackToMenuFromRetry()` inaczej funkcja która przenosi użytkownika do menu z poziomu sceny **Retry** do sceny **SplashScreen**.
```    public void GoBackToMenuFromRetry()
    {
        Debug.Log("Go back to Menu from Retry");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
```
Po powyższym fragmencie kodu widać, że tutaj również dodałem Log Debuggera, gdyż utknąłem przy tym przykładzie na chwilę. Jak się potem okazało, zapomniałem
wtedy dodać obsługę wydarzeń w Unity. Ostatnia linia kodu działa na takiej zasadzie, że przenosi nas scenę wstecz, czyli do sceny **SplashScreen**.

## Level01
Jest to według mnie najbardziej skomplikowana scena, w okół której skupiona jest cały ten program. Gdy użytkownik wejdzie w tę scenę, jego oczom ukaże się zielona
kula i plansza, a obiekt przypisany jako **Player** będzie sterowany przez niego za pomocą klawiszy **A** oraz **D**. Kula będzie się toczyć sama i będzie również
podskakwiać przy tym. Prawa fizyki również zostały zaimplementowane z programu Unity i warunkują one na to jak zachowa się przeszkoda oraz piłka w zderzeniu ze sobą.
Stworzyłem kilka osobnych tagów obiektów z czego można wyróżnić 3 główne: **Obstacle**, **Player**, **Finish**. Każdy tag otrzymał specjalny materiał. Obiekt
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
przypisany do tego skryptu. Nadpisałem również zmienne takie jak **fowardForce** (umknęło mi tutaj **r**), oraz **sidewaysForce**. Jest to prędkosć z jaką porusza się
piłka zarówno do przodu, jak i na boki. **fowardForce** jest odpowiedzialne za poruszanie się obiektu na osi Z, natomiast **sidewaysForce** jest odpowiedzialne za
poruszanie się na osi X. Również widać tutaj zaimplementowane z silnika Unity rozwiązanie na sterowanie kulą. W przypadku gdy użytkownik wprowadzi klawisz **d**,
wówczas kula zacznie przesuwać się w prawo (czyli o dodatnią wartość X), w przypadku gdy użytkownik wprowadzi klawisz **a**, wówczas kula zacznie przesuwać się w
lewo (czyli o ujemną wartość X). Jest tutaj również wyłapywanie wypadnięcia poza mapę. Bardziej łopatologicznie można stwierdzić, że w momencie kiedy pozycja gracza
osiągnie mniejszą wartość od **-1f** na płaszczyźnie Y, wówczas wywoła się polecenie `FindObjectOfType<GameManager>().EndGame();`, które zostało zaimplementowane z
silnika Unity.
