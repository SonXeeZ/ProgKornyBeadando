import datetime
import hashlib #Password titkosításához.

class DataBase:
    def __init__(self, filename): #DB Konstruktor
        self.filename = filename
        self.users = None
        self.file = None
        self.load_users()

    def load_users(self): #Userdata betöltése txt-ből.
        self.file = open(self.filename, "r")
        self.users = {}

        for line in self.file:
            email, password, name, created = line.strip().split(";")
            self.users[email] = (password, name, created)

        self.file.close()

    def get_user(self, email): #Lekérdezi hogy adott email-hez tartozik-e Acc.
        if email in self.users:
            return self.users[email]
        else:
            return -1

    def add_user(self, email, password, name): #Új User hozzáadása.
        if email.strip() not in self.users:
            self.users[email.strip()] = (hashlib.sha1(bytes(password.strip(), 'utf-8')).hexdigest(), name.strip(), DataBase.get_date())
            self.save_user()
            return 1
        else:
            print("Email address already exists.")
            return -1

    def remove_user(self, email): #User törlése.
        if email.strip() in self.users:
            del self.users[email.strip()]
            self.save_user()
            return 1
        else:
            print("User with this email does not exists.")
            return -1


    def modify_user(self, email1, email, password, name): #Meglévő User módosítása.
        if email1.strip() in self.users:
            self.remove_user(email1.strip())
            self.users[email.strip()] = (hashlib.sha1(bytes(password.strip(), 'utf-8')).hexdigest(), name.strip(), DataBase.get_date())
            self.save_user()
            return 1
        else:
            print("Cannot modify un-existing account")
            return -1

    def validate_user(self, email, password): #Megnézi hogy az email-password kombináció helyes-e.
        if self.get_user(email) != -1:
            return self.users[email][0] == hashlib.sha1(bytes(password, 'utf-8')).hexdigest()
        else:
            return False

    def save_user(self): #User mentése TXT-be.
        with open(self.filename, "w") as f:
            for user in self.users:
                f.write(user + ";" + self.users[user][0] + ";" + self.users[user][1] + ";" + self.users[user][2] + "\n")

    @staticmethod
    def get_date():
        return str(datetime.datetime.now()).split(" ")[0]