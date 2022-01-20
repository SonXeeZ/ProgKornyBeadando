from kivy.app import App
from kivy.lang import Builder
from kivy.uix.screenmanager import ScreenManager, Screen
from kivy.properties import ObjectProperty
from kivy.uix.popup import Popup
from kivy.uix.label import Label
from database import DataBase


class CreateAccountWindow(Screen): #Regisztrációs ablak.
    username = ObjectProperty(None)
    email = ObjectProperty(None)
    password = ObjectProperty(None)

    def submit(self): #Beírt adatok helyességének ellenőrzése.
        if self.username.text != "" and self.email.text != "" and self.email.text.count("@") == 1 and self.email.text.count(".") > 0:
            if self.password != "":
                db.add_user(self.email.text, self.password.text, self.username.text) #Ha megfelelő a paraméterezés, hozzáadjuk az adatbázishoz.

                self.reset()

                sm.current = "login" #Visszavezet a Login ablakra.
            else:
                pop_message('Invalid form', 'Please fill in all inputs with valid information.')
        else:
            pop_message('Invalid form', 'Please fill in all inputs with valid information.')

    def login(self): #A regisztrációs ablakból vissza tudunk lépni a Login fülre.
        self.reset()
        sm.current = "login" #Visszavezet a Login ablakra.

    def reset(self): #Reseteli a bemeneti mezőket.
        self.email.text = ""
        self.password.text = ""
        self.username.text = ""


class LoginWindow(Screen): #Login Ablak
    email = ObjectProperty(None)
    password = ObjectProperty(None)

    def login_button(self):#Validáljuk az információt, ha az email és a jelszó korrekt, eltároljuk az emailt hogy később tudjuk módosítani vagy törölni az Acc-ot.

        if db.validate_user(self.email.text, self.password.text):
            AccountWindow.current = self.email.text
            ModifyAccountWindow.current = self.email.text
            self.reset()
            sm.current = "account" #Visszavezet a Account ablakra.
        else:
            pop_message('Invalid Login', 'Invalid username or password.')


    def create_button(self): #A login ablakból át tudunk lépni a regisztrációs fülre.
        self.reset()
        sm.current = "create"

    def reset(self):
        self.email.text = ""
        self.password.text = ""


class AccountWindow(Screen): #Account ablak.
    username = ObjectProperty(None)
    created = ObjectProperty(None)
    email = ObjectProperty(None)
    current = ""

    def log_out(self):
        sm.current = "login"
        pop_message('Redirected to login screen', 'Successfully logged out.')

    def delete_account(self): #Törli a jelenlegi Accountot és visszadob a login ablakra.
        db.remove_user(self.current)
        sm.current = "login"
        pop_message('Redirected to login screen', 'Your account has been successfully deleted.')

    def on_enter(self, *args): #Kitölti a labeleket az információinkkal amikor az ablakra lépünk.
        password, name, created = db.get_user(self.current)
        self.username.text = "Account Name: " + name
        self.email.text = "Email: " + self.current
        self.created.text = "Created On: " + created


class ModifyAccountWindow(Screen): #Account módosítás ablak.
    username = ObjectProperty(None)
    created = ObjectProperty(None)
    email = ObjectProperty(None)
    current = ""

    def submit_modification(self): #Megnézi hogy a beírt paraméterek megfelelőek-e.
        if self.username.text != "" and self.email.text != "" and self.email.text.count(
                "@") == 1 and self.email.text.count(".") > 0:
            if self.password != "":
                db.modify_user(self.current, self.email.text, self.password.text, self.username.text) #Ha a paraméterek helyesek, frissítjük az Account információit az adatbázisban.

                self.reset()

                sm.current = "login"
                pop_message('Redirected to login screen', 'Your account has been successfully modified. Please log in again with your new credentials.')
            else:
                pop_message('Invalid Form', 'Please fill in all inputs with valid information.')
        else:
            pop_message('Invalid Form', 'Please fill in all inputs with valid information.')

    def reset(self):
        self.email.text = ""
        self.password.text = ""
        self.username.text = ""


class WindowManager(ScreenManager):
    pass


def pop_message(title, message): #Elküldi a felhasználó információt popup ablak segítségével.
    pop = Popup(title=title,
                  content=Label(text=message),
                  size_hint=(None, None), size=(650, 400))
    pop.open()


kv = Builder.load_file("login.kv") #Betölti a .kv file-t.

sm = WindowManager()
db = DataBase("users.txt") #Készítünk egy példányt a DataBase classból.

screens = [LoginWindow(name="login"), CreateAccountWindow(name="create"), AccountWindow(name="account"), ModifyAccountWindow(name="modify")] #All the different screens
for screen in screens:
    sm.add_widget(screen)

sm.current = "login"


class AccountManagerApp(App):
    def build(self):
        return sm


if __name__ == "__main__":
    AccountManagerApp().run()