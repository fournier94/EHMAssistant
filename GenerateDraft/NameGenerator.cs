using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;

namespace EHMAssistant
{
    class NameGenerator
    {
        #region Variables
        private static RandomNumberGenerator random;

        // Static Dictionaries for country-specific names
        private static Dictionary<CountryGenerator.Country, List<string>> firstNamesByCountry;
        private static Dictionary<CountryGenerator.Country, List<string>> lastNamesByCountry;

        // Set to track all used names globally
        private static HashSet<string> usedFirstNames;
        private static HashSet<string> usedLastNames;

        // Dictionary for recently used names
        private static Dictionary<CountryGenerator.Country, List<string>> recentlyUsedNames;
        #endregion

        #region Initialize lists of names by Country
        private static void InitializeNameLists()
        {
            #region First names by country
            firstNamesByCountry = new Dictionary<CountryGenerator.Country, List<string>>
    {
        {CountryGenerator.Country.Canada, new List<string> {
    "Aaron", "Adam", "Adrian", "Aidan", "Alexander", "Andrew", "Anthony", "Ashton", "Austin",
    "Benjamin", "Blake", "Brandon", "Brian", "Brody", "Caleb", "Cameron", "Carter", "Charles",
    "Chase", "Christopher", "Cole", "Connor", "Cooper", "Daniel", "David", "Declan", "Dominic",
    "Dylan", "Easton", "Elijah", "Elliot", "Ethan", "Evan", "Ezra", "Felix", "Finn", "Gabriel",
    "Gavin", "George", "Grayson", "Henry", "Hudson", "Hunter", "Isaac", "Jack", "Jackson",
    "Jacob", "James", "Jason", "Jasper", "Jayden", "Jeremy", "Jesse", "Jonah", "Jonathan",
    "Jordan", "Joseph", "Julian", "Kai", "Kevin", "Landon", "Levi", "Liam", "Logan", "Lucas",
    "Luke", "Malcolm", "Mason", "Matthew", "Max", "Michael", "Nathan", "Nathaniel", "Nicholas",
    "Nolan", "Oliver", "Owen", "Parker", "Patrick", "Paul", "Peter", "Philippe", "Quinn",
    "Raphael", "Reid", "Riley", "Robert", "Rowan", "Ryan", "Samuel", "Sawyer", "Sebastian",
    "Simon", "Spencer", "Stephen", "Theo", "Thomas", "Timothy", "Tristan", "Tyler", "Victor",
    "Vincent", "Wesley", "William", "Wyatt", "Xavier", "Zachary", "Zane", "Abel", "Abram",
    "Ace", "Albert", "Alden", "Alfred", "Amos", "Anderson", "Angus", "Archie", "Arthur",
    "Atticus", "Barrett", "Beau", "Benson", "Bennett", "Billy", "Bowen", "Brantley", "Brayden",
    "Brent", "Bruce", "Cade", "Callum", "Calvin", "Carson", "Cedric", "Chandler", "Clark",
    "Clayton", "Clifford", "Clyde", "Cody", "Colby", "Corey", "Cyrus", "Damian", "Dane",
    "Darius", "Darren", "Dawson", "Deacon", "Desmond", "Devon", "Dexter", "Donald", "Donovan",
    "Douglas", "Drew", "Elias", "Emerson", "Emmanuel", "Everett", "Finley", "Fletcher",
    "Forest", "Foster", "Frank", "Freddie", "Garrett", "Gordon", "Graham", "Grant", "Greyson",
    "Griffin", "Harold", "Harris", "Heath", "Holden", "Hugo", "Ian", "Ignacio", "Irving", "Ivan",
    "Jedidiah", "Jeffrey", "Jennings", "Jerome", "Joaquin", "Johan", "Kaden", "Kane", "Kendrick",
    "Lachlan", "Laurence", "Lawrence", "Leon", "Lionel", "Marco", "Maurice", "Milton"
}},
{CountryGenerator.Country.UnitedStates, new List<string> {
    "Aaron", "Adam", "Adrian", "Aiden", "Alexander", "Andrew", "Anthony", "Ashton", "Austin",
    "Benjamin", "Blake", "Brandon", "Brian", "Caleb", "Cameron", "Carter", "Charles", "Chase",
    "Christopher", "Cole", "Connor", "Cooper", "Daniel", "David", "Declan", "Dominic", "Dylan",
    "Easton", "Elijah", "Elliot", "Ethan", "Evan", "Ezra", "Felix", "Finn", "Gabriel", "Gavin",
    "George", "Grayson", "Harrison", "Henry", "Hudson", "Hunter", "Ian", "Isaac", "Jack", "Jackson",
    "Jacob", "James", "Jason", "Jasper", "Jayden", "Jeremy", "Jesse", "Jonah", "Jonathan", "Jordan",
    "Joseph", "Julian", "Kai", "Kevin", "Landon", "Levi", "Liam", "Logan", "Lucas", "Luke", "Malcolm",
    "Mason", "Matthew", "Max", "Michael", "Nathan", "Nathaniel", "Nicholas", "Nolan", "Oliver", "Owen",
    "Parker", "Patrick", "Paul", "Peter", "Phillip", "Quinn", "Raphael", "Reid", "Riley", "Robert",
    "Rowan", "Ryan", "Samuel", "Sawyer", "Sebastian", "Simon", "Spencer", "Stephen", "Theo", "Thomas",
    "Timothy", "Tristan", "Tyler", "Victor", "Vincent", "Wesley", "William", "Wyatt", "Xavier", "Zachary",
    "Zane", "Abel", "Abram", "Ace", "Albert", "Alden", "Alfred", "Amos", "Anderson", "Angus", "Archie",
    "Arthur", "Atticus", "Barrett", "Beau", "Benson", "Bennett", "Billy", "Bowen", "Brantley", "Brayden",
    "Brent", "Bruce", "Cade", "Callum", "Calvin", "Carson", "Cedric", "Chandler", "Clark", "Clayton",
    "Clifford", "Clyde", "Cody", "Colby", "Corey", "Cyrus", "Damian", "Dane", "Darius", "Darren", "Dawson",
    "Deacon", "Desmond", "Devon", "Dexter", "Donald", "Donovan", "Douglas", "Drew", "Elias", "Emerson",
    "Emmanuel", "Everett", "Finley", "Fletcher", "Forest", "Foster", "Frank", "Freddie", "Garrett", "Gordon",
    "Graham", "Grant", "Greyson", "Griffin", "Harold", "Harris", "Heath", "Holden", "Hugo", "Ignacio",
    "Irving", "Ivan", "Jedidiah", "Jeffrey", "Jennings", "Jerome", "Joaquin", "Johan", "Kaden", "Kane",
    "Kendrick", "Lachlan", "Leon", "Lionel", "Marco", "Maurice", "Milton", "Nelson", "Otis", "Warren"
}},
{CountryGenerator.Country.Sweden, new List<string> {
    "Albin", "Alexander", "Andreas", "Anton", "Arvid", "Axel", "Benjamin", "Bill", "Billy", "Bob", "Bo", "Carl", "Christian", "Daniel", "David", "Dennis", "Edvin", "Elias", "Emil", "Erik", "Eskil", "Fabian", "Felix", "Filip", "Fredrik", "Gabriel", "Gustav", "Hans", "Henrik", "Hugo", "Hakan", "Isak", "Ivan", "Jack", "Jakob", "Jan", "Jesper", "Joel", "John", "Jonas", "Jonathan", "Jorgen", "Josef", "Karl", "Kevin", "Knut", "Lars", "Leo", "Liam", "Linus", "Ludwig", "Lukas", "Magnus", "Malte", "Marcus", "Martin", "Mattias", "Max", "Melker", "Mikael", "Nils", "Noel", "Oskar", "Otto", "Par", "Patrik", "Paul", "Rasmus", "Robin", "Samuel", "Sebastian", "Sigvard", "Simon", "Stefan", "Sten", "Sven", "Theodor", "Theo", "Thomas", "Tim", "Tobias", "Tommy", "Viktor", "Ville", "Wilhelm", "William", "Yngve", "Algot", "Alf", "Alvar", "Anders", "Andrei", "Anton", "Arne", "Artur", "Aslak", "August", "Bastian", "Bertil", "Birger", "Bo", "Borje", "Calle", "Caspian", "Christer", "Clarence", "Clement", "Dag", "Dan", "David", "Dennis", "Edvard", "Elis", "Emiliano", "Enzo", "Evald", "Frans", "Fredrik", "Gabriel", "Georg", "Gosta", "Greger", "Gunnar", "Halvard", "Hamid", "Hans", "Harry", "Henrik", "Herman", "Hjalmar", "Ingmar", "Ingvar", "Isidore", "Ivar", "Jack", "Jarl", "Jasper", "Jerker", "Jim", "Jorgen", "Jorgen", "Jorgen", "Joakim", "Johan", "Johnny", "Jonas", "Jorgen", "Josef", "Julius", "Justus", "Kai", "Karl", "Lennart", "Leif", "Lennart", "Lucas", "Ludvig", "Magnus", "Marius", "Mark", "Martin", "Melvin", "Marten", "Max", "Mikael", "Milos", "Nils", "Niklas", "Noel", "Olle", "Oskar", "Pelle", "Par", "Ralf", "Ralph", "Reidar", "Robert", "Roger", "Rune", "Rurik", "Samuel", "Sebastian", "Simon", "Steffan", "Sten", "Svante", "Svein", "Tobias", "Tom", "Tommy", "Torbjorn", "Ulf", "Urban", "Victor", "Viktor", "Wilhelm", "William", "Yngve", "Zander", "Zet"
}},
{CountryGenerator.Country.Russia, new List<string> {
    "Aaron", "Aleksandr", "Aleksei", "Anatoly", "Andrei", "Arkady", "Artur", "Boris", "Bulat", "Vadim", "Valentin", "Valery", "Vasily", "Viktor", "Vitaly", "Vladislav", "Vladimir", "Vyacheslav", "Yaroslav", "Yegor", "Yuri", "Adrik", "Alexey", "Artem", "Basil", "Boris", "Denis", "Dmitry", "Evgeny", "Fedor", "Fyodor", "Gennady", "Georgy", "Grigory", "Igor", "Ilia", "Ilya", "Konstantin", "Kirill", "Lev", "Luka", "Maksim", "Matvei", "Mikhail", "Nikolai", "Oleg", "Pavel", "Pyotr", "Roman", "Ruslan", "Semyon", "Sergei", "Stanislav", "Timofey", "Vadim", "Veniamin", "Vladimir", "Vyacheslav", "Yegor", "Yuri", "Aleksej", "Aleksei", "Anastasia", "Boris", "Vasiliev", "Viktor", "Andrey", "Arkadi", "Anton", "Artem", "Bogatyr", "Cyril", "David", "Dmitri", "Danila", "Egor", "Fedor", "Feofan", "Gavril", "Grisha", "Georgy", "Gleb", "Igor", "Ilia", "Igor", "Irina", "Ilya", "Kirill", "Lev", "Leonid", "Maksim", "Mikhail", "Nikita", "Pavel", "Roman", "Rodion", "Sergey", "Stepan", "Semyon", "Timofey", "Vlad", "Valera", "Vitaliy", "Vladislav", "Vladimir", "Viktor", "Vova", "Boris", "Konstantin", "Sergio", "Vasily", "Vladislav", "Alexey", "Valery", "Zakhar", "Victor", "Yuriy", "Yakov", "Andrei", "Vladislav", "Lev", "Mikhail", "Aleksey", "Alexander", "Yevgeny", "Dmitry", "Dima", "Nikolay", "Roman", "Viktor", "Vasily", "Aleksei", "Nikita", "Igor", "Sergei", "Semen", "Yuri", "Timofey", "Dmitriy", "Vlad", "Maksim", "Maxim", "Aleksej", "Leonid", "Maksym", "Vasily", "Vova", "Oleg", "Sergey", "Vlad", "Semyon", "Evgeny", "Alya", "Boris", "Roman", "Vadim", "Fyodor", "Viktor", "Boris", "Artyom", "Maksim", "Dmitri", "Mikhail", "Pavel", "Semyon", "Vitaly", "Matvei", "Vladimir", "Zakhar", "Yegor", "Igor", "Sergei", "Roman", "Yuri", "Maxim", "Victor", "Alexandr", "Alexey", "Arkadi", "Boris", "Denis", "Dmitry", "Evgeny", "Fedor", "Georgy", "Igor", "Ilya", "Konstantin", "Kirill", "Lev", "Matvei", "Maksim", "Mikhail", "Nikolai", "Oleg", "Pavel", "Roman", "Ruslan", "Semyon", "Sergei", "Stanislav", "Timofey", "Vadim", "Veniamin", "Vladimir", "Vyacheslav", "Yegor", "Yuri"
}},
{CountryGenerator.Country.Finland, new List<string> {
    "Aarno", "Ahti", "Aleksi", "Alpo", "Antero", "Armas", "Arto", "Eemil", "Eero", "Elias", "Eljas", "Enrico", "Erik", "Esa", "Esko", "Eino", "Helge", "Herman", "Hannu", "Harri", "Heikki", "Hille", "Ilari", "Iiro", "Ilkka", "Jari", "Jaska", "Jere", "Johan", "Joonas", "Joona", "Jukka", "Juha", "Jussi", "Kalle", "Kari", "Kasper", "Kaspar", "Kimi", "Kimmo", "Kosti", "Kullervo", "Lassi", "Leevi", "Lenni", "Leo", "Lauri", "Lasse", "Matti", "Mikko", "Mikael", "Mika", "Niko", "Nils", "Nuutti", "Olli", "Onni", "Orvo", "Otto", "Paavo", "Pekka", "Petrus", "Petri", "Raimo", "Riku", "Risto", "Robin", "Roland", "Sakari", "Sami", "Samuel", "Simo", "Tapio", "Tero", "Teemu", "Timo", "Tommi", "Toni", "Veli", "Veikko", "Vesa", "Vili", "Vilho", "Viljami", "Viljo", "Vaino", "Vesa", "Eetu", "Eelis", "Eeli", "Elmeri", "Erkki", "Esko", "Gunnar", "Harald", "Hermann", "Hannes", "Hannu", "Iivari", "Iiro", "Ilmo", "Ilkka", "Jaakko", "Jalmari", "Juhani", "Joni", "Jouko", "Jussi", "Juha", "Jarkko", "Jukka", "Jouko", "Jari", "Kimmo", "Kalle", "Kari", "Kasper", "Kimi", "Kosti", "Kurt", "Lauri", "Lasse", "Lassi", "Leo", "Leevi", "Lenni", "Mauri", "Mikko", "Mats", "Mikael", "Mika", "Niko", "Noel", "Niklas", "Oskari", "Otto", "Pekka", "Raimo", "Risto", "Riku", "Rauno", "Sami", "Samuel", "Sakari", "Simo", "Tapio", "Teemu", "Timo", "Veli", "Veikko", "Vesa", "Vili", "Vilho", "Viljami", "Viljo", "Vaino", "Aaro", "Aimo", "Antti", "Atte", "Bo", "Einari", "Elias", "Eljas", "Enzo", "Gunnar", "Heikki", "Henrik", "Herman", "Hannari", "Ismo", "Ilmo", "Jani", "Jari", "Jarmo", "Jussi", "Jukka", "Juuso", "Kai", "Kasper", "Kosti", "Kalle", "Kimmo", "Lauri", "Matti", "Mikko", "Mika", "Olli", "Onni", "Otto", "Paavo", "Pekka", "Raimo", "Rauno", "Sakari", "Simo", "Teemu", "Timo", "Toni", "Veli", "Vesa", "Vili", "Viljo", "Veikko", "Vaino"
}},
{CountryGenerator.Country.CzechRepublic, new List<string> {
    "Adam", "Adolf", "Alois", "Andrej", "Antonin", "Arnost", "Bohdan", "Bohuslav", "Bohumil", "Boris", "Bretislav", "Cyril", "Dalibor", "Daniel", "David", "Dionyz", "Dominik", "Dusan", "Edgar", "Emanuel", "Emil", "Erik", "Filip", "Frantisek", "Gabriel", "Gustav", "Hynek", "Igor", "Ivo", "Jakub", "Jarek", "Jaroslav", "Jindrich", "Jiri", "Josef", "Jozef", "Julius", "Karel", "Kamil", "Karol", "Klement", "Krystof", "Ladislav", "Leo", "Lukas", "Lubos", "Lubomir", "Marek", "Martin", "Michal", "Milos", "Miroslav", "Mojmir", "Nikolaj", "Oldrich", "Otakar", "Pavel", "Patrik", "Petr", "Radek", "Radoslav", "Richard", "Robert", "Roman", "Rostislav", "Simon", "Stanislav", "Stepan", "Tadeas", "Tibor", "Tomas", "Vaclav", "Vaclav", "Valentyn", "Viktor", "Vit", "Vladimir", "Zdenek", "Zoran", "Adela", "Alfred", "Alzbeta", "Andrea", "Bohuslav", "Borek", "Bretislav", "Cyril", "Dalibor", "David", "Dionyz", "Dominik", "Dusan", "Edgar", "Emanuel", "Emil", "Erik", "Filip", "Frantisek", "Gabriel", "Gustav", "Hynek", "Igor", "Ivo", "Jakub", "Jarek", "Jaroslav", "Jindrich", "Jiri", "Josef", "Jozef", "Julius", "Karel", "Kamil", "Karol", "Klement", "Krystof", "Ladislav", "Leo", "Lukas", "Lubos", "Lubomir", "Marek", "Martin", "Michal", "Milos", "Miroslav", "Mojmir", "Nikolaj", "Oldrich", "Otakar", "Pavel", "Patrik", "Petr", "Radek", "Radoslav", "Richard", "Robert", "Roman", "Rostislav", "Simon", "Stanislav", "Stepan", "Tadeas", "Tibor", "Tomas", "Vaclav", "Vaclav", "Valentyn", "Viktor", "Vit", "Vladimir", "Zdenek", "Zoran", "Adela", "Alfred", "Alzbeta", "Andrea", "Borek", "Bohumil", "Bohuslav", "Cyril", "Dalibor", "David", "Dionyz", "Dominik", "Dusan", "Emil", "Filip", "Frantisek", "Gabriel", "Gustav", "Hynek", "Igor", "Ivo", "Jakub", "Jarek", "Jaroslav", "Jindrich", "Jiri", "Josef", "Jozef", "Karel", "Kamil", "Klement", "Krystof", "Ladislav", "Leo", "Lukas", "Lubos", "Lubomir", "Marek", "Martin", "Milos", "Miroslav", "Mojmir", "Nikolaj", "Oldrich", "Otakar", "Pavel", "Patrik", "Petr", "Radek", "Radoslav", "Richard", "Robert", "Roman", "Rostislav", "Simon", "Stanislav", "Stepan", "Tadeas", "Tibor", "Tomas", "Vaclav", "Valentyn", "Viktor", "Vit", "Vladimir", "Zdenek"
}},
{CountryGenerator.Country.Slovakia, new List<string> {
    "Adam", "Adrian", "Albert", "Ales", "Alfonz", "Andrej", "Anton", "Arpad", "Augustin", "Benedikt", "Bohdan", "Bohuslav", "Boris", "Branislav", "Bronislav", "Cyril", "Dalibor", "Daniel", "David", "Denis", "Dionyz", "Dominik", "Dusan", "Eduard", "Emil", "Erik", "Filip", "Frantisek", "Gabriel", "Gustav", "Henrich", "Igor", "Jan", "Jakub", "Jozef", "Juraj", "Kamil", "Karol", "Katarin", "Klement", "Ladislav", "Lukas", "Marek", "Martin", "Milos", "Miroslav", "Mojmir", "Nikolaj", "Norbert", "Oldrich", "Ondrej", "Patrik", "Pavol", "Peter", "Petr", "Radoslav", "Rafael", "Robert", "Roman", "Samson", "Stanislav", "Simon", "Tadeas", "Tibor", "Tomas", "Vaclav", "Valentin", "Viktor", "Vladimir", "Zdenek", "Zoran", "Vit", "Vojtech", "Adrian", "Alfred", "Alzbeta", "Bohumil", "Boris", "Cyril", "Dalibor", "David", "Dionyz", "Dominik", "Dusan", "Emil", "Filip", "Frantisek", "Gabriel", "Gustav", "Hynek", "Igor", "Ivo", "Jakub", "Jarek", "Jaroslav", "Jindrich", "Jiri", "Josef", "Jozef", "Julius", "Karel", "Kamil", "Karol", "Klement", "Krystof", "Ladislav", "Leo", "Lukas", "Lubos", "Lubomir", "Marek", "Martin", "Michal", "Milos", "Miroslav", "Mojmir", "Nikolaj", "Oldrich", "Otakar", "Pavel", "Patrik", "Petr", "Radek", "Radoslav", "Richard", "Robert", "Roman", "Rostislav", "Simon", "Stanislav", "Stepan", "Tadeas", "Tibor", "Tomas", "Vaclav", "Vaclav", "Valentyn", "Viktor", "Vit", "Vladimir", "Zdenek", "Zoran", "Adela", "Alfred", "Alzbeta", "Andrea", "Borek", "Bohumil", "Bohuslav", "Cyril", "Dalibor", "David", "Dionyz", "Dominik", "Dusan", "Emil", "Filip", "Frantisek", "Gabriel", "Gustav", "Hynek", "Igor", "Ivo", "Jakub", "Jarek", "Jaroslav", "Jindrich", "Jiri", "Josef", "Jozef", "Karel", "Kamil", "Klement", "Krystof", "Ladislav", "Leo", "Lukas", "Lubos", "Lubomir", "Marek", "Martin", "Milos", "Miroslav", "Mojmir", "Nikolaj", "Oldrich", "Otakar", "Pavel", "Patrik", "Petr", "Radek", "Radoslav", "Richard", "Robert", "Roman", "Rostislav", "Simon", "Stanislav", "Stepan", "Tadeas", "Tibor", "Tomas", "Vaclav", "Valentyn", "Viktor", "Vit", "Vladimir", "Zdenek", "Zoran"
}},
{CountryGenerator.Country.Switzerland, new List<string> {
    "Aaron", "Adrian", "Alain", "Albert", "Alex", "Alexander", "Alessandro", "Andreas", "Anton", "Benedikt", "Benjamin", "Benoit", "Bernard", "Bruno", "Cedric", "Charles", "Christian", "Christoph", "Clement", "Daniel", "David", "Dominik", "Eric", "Elias", "Emil", "Felix", "Florian", "Frederic", "Gabriel", "Gerard", "Giovanni", "Guido", "Gustav", "Hans", "Heinrich", "Hermann", "Hugo", "Igor", "Jakob", "Jan", "Jerome", "Joel", "Joris", "Joseph", "Julius", "Klaus", "Luca", "Luc", "Lukas", "Marc", "Marco", "Martin", "Matthias", "Max", "Michael", "Milan", "Nicolas", "Nils", "Noe", "Olivier", "Patrick", "Paul", "Philipp", "Pierre", "Quentin", "Raphael", "Remy", "Reto", "Robin", "Rocco", "Roland", "Samuel", "Simon", "Stefan", "Stephan", "Sven", "Thomas", "Tim", "Tobias", "Valentin", "Victor", "Vincent", "Walter", "Werner", "Yannick", "Zeno", "Adriano", "Albert", "Alessio", "Amadeo", "Armin", "Bastian", "Benoit", "Bernardo", "Boris", "Cedric", "Claudio", "Dario", "Davide", "Denis", "Enzo", "Fabian", "Felipe", "Florian", "Frederic", "Gael", "Gian", "Gianluca", "Giulio", "Guido", "Hugo", "Ilan", "Ismael", "Janos", "Jeremy", "Joel", "Jonas", "Jules", "Jurgen", "Kilian", "Leon", "Luca", "Lorenzo", "Luciano", "Luis", "Lukas", "Marius", "Marvin", "Matteo", "Maurizio", "Maxim", "Michel", "Michele", "Mickael", "Milan", "Morten", "Nino", "Nicolas", "Nils", "Olivier", "Otto", "Pascal", "Patrice", "Raphael", "Remo", "Ricardo", "Romain", "Ruben", "Samuel", "Sandro", "Sandro", "Sebastien", "Sven", "Theo", "Theo", "Thibault", "Timothy", "Tobias", "Tomas", "Toni", "Valentin", "Vincenzo", "Virgil", "Walter", "Werner", "Wilhelm", "Yves", "Zack", "Zeno", "Zoran", "Zuhair", "Zaki", "Aurelio", "Beni", "Eliot", "Franz", "Gert", "Leandro", "Michele", "Neven", "Sacha", "Timo", "Zdenek"
}},
{CountryGenerator.Country.Germany, new List<string> {
    "Aaron", "Adrian", "Alexander", "Andreas", "Angelo", "Anton", "Bastian", "Ben", "Benjamin", "Bernhard", "Bjorn", "Blake", "Bodo", "Bruno", "Carl", "Christian", "Christoph", "Daniel", "David", "Dennis", "Dominik", "Dorian", "Elias", "Emil", "Felix", "Florian", "Frank", "Frederik", "Gabriel", "Gerd", "Gunter", "Hans", "Heiko", "Helmut", "Henrik", "Hermann", "Holger", "Hubert", "Ignaz", "Jakob", "Jannik", "Jan", "Jens", "Johann", "Jonathan", "Jorg", "Julius", "Kai", "Karl", "Klaus", "Kristian", "Lars", "Leon", "Luca", "Lucas", "Lukas", "Malte", "Manuel", "Marco", "Markus", "Martin", "Matthias", "Max", "Michael", "Milan", "Moritz", "Niklas", "Noah", "Norbert", "Oliver", "Oskar", "Pascal", "Patrick", "Paul", "Peter", "Philipp", "Rafael", "Rainer", "Rasmus", "Reiner", "Rene", "Richard", "Robert", "Rolf", "Roman", "Ruben", "Samuel", "Sascha", "Sebastian", "Simon", "Stefan", "Steffen", "Sven", "Timo", "Tim", "Tobias", "Udo", "Ulrich", "Uwe", "Valentin", "Victor", "Vincent", "Werner", "Willi", "Wilhelm", "Yannik", "Yves", "Zeno", "Zoran", "Adriano", "Alfred", "Amos", "Arvid", "Axel", "Bert", "Benedikt", "Bernd", "Berndt", "Boris", "Clemens", "Dieter", "Eckhard", "Egon", "Erik", "Ernst", "Friedrich", "Georg", "Gerald", "Gotz", "Gunther", "Hanno", "Hendrik", "Herbert", "Hermann", "Horst", "Hubertus", "Igor", "Ivo", "Jurgen", "Karin", "Kay", "Lennart", "Leonard", "Ludwig", "Manfred", "Marek", "Mark", "Nico", "Nikolas", "Olivier", "Otmar", "Otto", "Peter-Paul", "Ralf", "Rudolf", "Stephan", "Siegfried", "Thorsten", "Tobias", "Tomas", "Volker", "Walter", "Wendelin", "Werner", "Zbigniew", "Achim", "Alban", "Berthold", "Dieter", "Edgar", "Felix", "Gustav", "Herbert", "Holger", "Horst", "Jan", "Joachim", "Kurt", "Lennart", "Leopold", "Manuel", "Maximilian", "Norwin", "Oswald", "Reinhold", "Stefan", "Siegfried", "Sven", "Theo", "Thilo", "Volker", "Wilfried", "Wolfram", "Xaver", "Zacharias"
}},
{CountryGenerator.Country.Denmark, new List<string> {
    "Adam", "Albert", "Alexander", "Anders", "Andreas", "Arne", "Asger", "August", "Benjamin", "Bengt", "Benny", "Bent", "Bo", "Boris", "Carl", "Christian", "Christopher", "Claes", "Claus", "Daniel", "David", "Dennis", "Emil", "Erik", "Felix", "Finn", "Frederik", "Frede", "Gabriel", "Gustav", "Hans", "Harald", "Hassan", "Helge", "Henrik", "Holger", "Ivar", "Jakob", "Jens", "Joachim", "Johan", "Jorgen", "Karl", "Kenneth", "Klaus", "Kristian", "Lars", "Laurits", "Leif", "Leo", "Lucas", "Mads", "Magnus", "Malte", "Martin", "Mikkel", "Morten", "Nikolaj", "Nils", "Niels", "Ole", "Oscar", "Patrick", "Paul", "Per", "Peter", "Poul", "Rasmus", "Rene", "Richard", "Robert", "Rolf", "Rune", "Sebastian", "Simon", "Soren", "Stefan", "Steffen", "Stig", "Thorbjorn", "Thorvald", "Thomas", "Tobias", "Valdemar", "Victor", "Viggo", "William", "Yannik", "Age", "Albertus", "Alf", "Alfred", "Andre", "Anker", "Arvid", "Axel", "Bjarne", "Boas", "Boris", "Bastian", "Benjamin", "Benedict", "Bentley", "Bjorn", "Brian", "Carlton", "Carlson", "Casper", "Christian", "Clint", "Clausen", "Davidson", "Ebbe", "Eckhard", "Elias", "Emil", "Ernst", "Fabian", "Frederik", "Gunnar", "Herman", "Hugo", "Igor", "Jacob", "Jakobsen", "Jens", "Joachim", "Johannes", "Jonas", "Kasper", "Klaus", "Kristoffer", "Kurt", "Lennart", "Lukas", "Magnus", "Marius", "Morten", "Nikolai", "Nils", "Noah", "Olaf", "Otto", "Peder", "Pelle", "Poul", "Ralf", "Rasmus", "Reinhold", "Rene", "Sander", "Sigurd", "Steen", "Soren", "Thor", "Thorsten", "Thure", "Tim", "Tobias", "Toke", "Valdemar", "Valter", "Viggo", "Villy", "Wilhelm", "Zacharias", "Alwin", "Amos", "Axel", "Bastian", "Bernd", "Boaz", "Borje", "Calvin", "Claudius", "Davidson", "Dennis", "Edvin", "Emmanuel", "Ezekiel", "Finn", "Frans", "Frej", "Gabriel", "George", "Gregers", "Guido", "Gunnar", "Harald", "Hugo", "Iver", "Jannik", "Jarl", "John", "Jonas", "Julian", "Jorgen", "Kian", "Klaus", "Kristian", "Lars", "Lennard", "Mads", "Magnus", "Mikkel", "Niels", "Noe", "Nicolai", "Oscar", "Palle", "Patrik", "Peder", "Per", "Rasmus", "Reinhard", "Rene", "Rico", "Robert", "Rune", "Sebastian", "Stefan", "Sven", "Thibaut", "Theo", "Thore", "Thorvald", "Thorsten", "Tobias", "Toke", "Tomas", "Valter", "Victor", "Vilhelm", "Yannick", "Zackarias"
}},
{CountryGenerator.Country.Latvia, new List<string> {
    "Aigars", "Ainars", "Alberts", "Aleksandrs", "Alfreds", "Andris", "Antons", "Arturs", "Arvis", "Atskats", "Augusts", "Baiba", "Baltars", "Bens", "Bertrams", "Blevs", "Dainis", "Daniels", "Davis", "Dainis", "Dainis", "Dainis", "Deniss", "Didzis", "Edgars", "Eduards", "Elvis", "Emils", "Eriks", "Evarts", "Feliks", "Filips", "Gatis", "Georgs", "Gints", "Grigoris", "Gunars", "Guntis", "Haralds", "Heinrihs", "Hermanis", "Ilgvars", "Ilmars", "Indulis", "Inesis", "Ivars", "Janis", "Jazeps", "Jekabs", "Juris", "Juris", "Juris", "Kaspars", "Karlis", "Kristaps", "Krisjanis", "Kristers", "Kristians", "Krisjanis", "Laimons", "Latvis", "Livs", "Martins", "Matiss", "Mihails", "Mikus", "Martins", "Martins", "Modris", "Nauris", "Niks", "Niklavs", "Normunds", "Orests", "Olafs", "Peteris", "Peteris", "Raimonds", "Raivis", "Rihards", "Ritvars", "Roberts", "Rolands", "Rudolfs", "Rudolfs", "Savis", "Sandis", "Sarms", "Sarmis", "Sevs", "Sidriks", "Sigismunds", "Silvans", "Simons", "Sinbergs", "Skudra", "Solomons", "Svetais", "Talis", "Talis", "Toms", "Toms", "Toms", "Vaidis", "Valdis", "Valerijs", "Viktors", "Viesturs", "Vilnis", "Zigmars", "Zirnis", "Aigars", "Ainars", "Aleksandrs", "Andris", "Antons", "Arvis", "Augusts", "Baiba", "Bens", "Bertrams", "Dainis", "Daniels", "Davis", "Deniss", "Didzis", "Edgars", "Eduards", "Elvis", "Emils", "Eriks", "Evarts", "Feliks", "Filips", "Gatis", "Georgs", "Gints", "Grigoris", "Gunars", "Guntis", "Haralds", "Heinrihs", "Hermanis", "Ilgvars", "Ilmars", "Indulis", "Inesis", "Ivars", "Janis", "Jazeps", "Jekabs", "Juris", "Kaspars", "Karlis", "Kristaps", "Krisjanis", "Kristers", "Kristians", "Krisjanis", "Laimons", "Latvis", "Livs", "Martins", "Matiss", "Mihails", "Mikus", "Modris", "Nauris", "Niks", "Niklavs", "Normunds", "Orests", "Olafs", "Peteris", "Raimonds", "Raivis", "Rihards", "Ritvars", "Roberts", "Rolands", "Rudolfs", "Savis", "Sandis", "Sarms", "Sevs", "Sidriks", "Sigismunds", "Silvans", "Simons", "Sinbergs", "Skudra", "Solomons", "Svetais", "Talis", "Toms", "Vaidis", "Valdis", "Viktors", "Viesturs", "Vilnis", "Zigmars", "Zirnis"
}},
{CountryGenerator.Country.Belarus, new List<string> {
    "Aleksei", "Aleksey", "Andrei", "Anton", "Artyom", "Bogdan", "Dmitri", "Dmitriy", "Evgeny", "Gennadi", "Gleb", "Igor", "Ilya", "Ivan", "Kirill", "Konstantin", "Lev", "Maksim", "Mikhael", "Nikolai", "Pavel", "Roman", "Sergei", "Stanislav", "Vadim", "Vasily", "Viktor", "Vladimir", "Yaroslav", "Yevgeny", "Yuri", "Alexander", "Alexandr", "Anatoly", "Boris", "Vladislav", "Vyacheslav", "Fedor", "Filip", "Grigory", "Evgeniy", "Igory", "Leonid", "Makar", "Maksym", "Mihail", "Miron", "Nikita", "Oleg", "Petr", "Pyotr", "Ruslan", "Semyon", "Stepan", "Timofey", "Vlad", "Vsevolod", "Yevhen", "Yevhenii", "Zahar", "Zakhar", "Andriy", "Denis", "Vladimir", "Viktori", "Vladislav", "Bohdan", "Viktorya", "Gennadiy", "Petr", "Egor", "Zhenya", "Sergiy", "Illya", "Alexei", "Artem", "Bogdan", "Vadim", "Dimitri", "Maksym", "Mikola", "Sergii", "Victor", "Yuriy", "Dmytro", "Yevhen", "Igor", "Stefany", "Dmitriy", "Sergey", "Volodymyr", "Valeriy", "Yaroslav", "Alexey", "Boris", "Kiril", "Vladislav", "Aleksej", "Yuri", "Maksim", "Alexandr", "Leonid", "Filipp", "Pavlo", "Tymur", "Yuri", "Matvei", "Artur", "Aleksei", "Bogdan", "Alex", "Gleb", "Andrey", "Artyom", "Aynur", "Anton", "Fedor", "Yevheniy", "Vasiliy", "Dmitriy", "Serhiy", "Kirill", "Maksim", "Yuriy", "Roman", "Semyon", "Vadym", "Bohdan", "Ihor", "Svyatoslav", "Viktor", "Yaroslav", "Oleksandr", "Boris", "Igor", "Sergey", "Yevheniy", "Yuriy", "Serhiy", "Denis", "Roman", "Artem", "Leonid", "Viktor", "Oleg", "Yuriy", "Alexandr", "Petr", "Denys", "Vadym", "Mykhailo", "Dmytro", "Maxim", "Maksym", "Roman", "Mykola", "Oleksandr", "Stepan", "Ilya", "Viktoriya", "Alexey", "Yevhen", "Vasiliy", "Bohdan", "Tymur", "Roman", "Artyom", "Maksym", "Vasiliy", "Maxim", "Denis", "Igor", "Maksym", "Oleksandr", "Roman", "Semyon", "Pavlo", "Kirill", "Valeriy", "Vadym", "Yaroslav", "Mykhailo", "Yuriy", "Igor", "Viktor", "Artem", "Maksym", "Boris"
}},
{CountryGenerator.Country.Slovenia, new List<string> {
    "Ales", "Andrej", "Anze", "Blaz", "Bostjan", "Damjan", "Domen", "Dusan", "Emil", "Franci", "Gregor", "Igor", "Janko", "Jasper", "Jernej", "Joze", "Jonas", "Jure", "Klemen", "Kosta", "Luka", "Matevz", "Marko", "Martin", "Matic", "Matjaz", "Miha", "Milan", "Mihael", "Mihailo", "Niko", "Nikola", "Patrik", "Peter", "Rafael", "Rok", "Saso", "Simon", "Sven", "Tadej", "Tomaz", "Vasilij", "Vid", "Vladimir", "Zoran", "Zvonko", "Adrian", "Alojz", "Aljaz", "Alen", "Ales", "Amadej", "Andreja", "Anton", "Aurelij", "Bernard", "Benjamin", "Benedikt", "Bojan", "Borut", "Boris", "Branko", "Branislav", "David", "Denis", "Dejan", "Dimitrij", "Domagoj", "Edvard", "Erik", "Feri", "Franc", "Francisek", "Gvido", "Hrvoje", "Iztok", "Jaka", "Jakob", "Janko", "Jasmin", "Jost", "Jure", "Karel", "Klement", "Ladislav", "Leos", "Luka", "Lukas", "Luko", "Maks", "Maksim", "Mark", "Markus", "Matija", "Matvij", "Miro", "Misa", "Mitja", "Nace", "Natan", "Nedeljko", "Nejc", "Niko", "Omar", "Patrik", "Pavle", "Rene", "Rok", "Roland", "Samir", "Sebastian", "Silvan", "Simeon", "Stanko", "Svetozar", "Tadej", "Tihomir", "Timotej", "Tomaz", "Tomislav", "Urban", "Vanja", "Vasilij", "Vinko", "Vladimir", "Vlado", "Vlado", "Zdenko", "Ziga", "Zoran", "Zlatko", "Bostjan", "Branislav", "Dragan", "Damjan", "Boris", "Bosko", "Benjamin", "Davor", "Gorazd", "Igor", "Luka", "Maksim", "Damjan", "Davorin", "Andrej", "Aljaz", "Nikola", "Tihomir", "Viktor", "Zdenek", "Goran", "Ivan", "Dinko", "Darko", "Bozo", "Rado", "Sandi", "Blaz", "Filip", "Vladimir", "Igor", "Joze", "Robert", "Dejan", "Rok", "Jakob", "Vito", "Rudi", "Milan", "Jakov", "Niko", "Roman", "Samir", "Tadej", "Jan", "Natan", "Samo", "Omar", "Nejc", "Urban", "Franjo", "Matija", "Petar", "Jasmin", "Jure", "Mihael", "Klemen", "Matic", "Rudi", "Vito", "Gregor", "Sebastian", "Dejan", "Marko", "Saso", "Boris", "Markus", "Mitja"
}} };
            #endregion

            #region Last names by country
            lastNamesByCountry = new Dictionary<CountryGenerator.Country, List<string>>
    {
{CountryGenerator.Country.Canada, new List<string> {
    "Abadie", "Benoit", "Bergeron", "Bernier", "Bouchard", "Bourassa", "Brouillette", "Brunet", "Cameron", "Chabot",
    "Charbonneau", "Chartier", "Charest", "Cloutier", "Cote", "D'Amours", "Desjardins", "Dube", "Dumont", "Dupuis",
    "Gagne", "Gauthier", "Gendron", "Gibeault", "Girard", "Godin", "Gosselin", "Grenier", "Guerin", "Laflamme",
    "Lafleur", "Lamarche", "Lemoine", "Lemoine", "Lemoine", "Lavoie", "Leclerc", "Lefebvre", "Lemieux", "Lepine",
    "Levesque", "Lavigne", "Leduc", "Lemieux", "Lemoine", "Levesque", "Lefebvre", "Lafleur", "Laflamme", "Lavigne",
    "Lemieux", "Lemoine", "Levesque", "Lefebvre", "Gauthier", "Leclerc", "Leduc", "Leclerc", "Levesque", "Lepine",
    "Menard", "Michaud", "Menard", "Morin", "Nadeau", "Nault", "Ouellet", "Perron", "Plante", "Poisson",
    "Proulx", "Racine", "Rivard", "Robitaille", "Roy", "Simard", "Tanguay", "Thibeault", "Tremblay", "Vachon",
    "Vallieres", "Vezina", "Wheeler", "Yelle", "Ziegler", "Allard", "Angers", "Archambault", "Arsenault", "Belanger",
    "Bergeron", "Boucher", "Bouchard", "Bourque", "Caron", "Charest", "Charbonneau", "Chauvin", "Cote", "David",
    "Dufresne", "Ducharme", "Dufour", "Durand", "Duval", "Ethier", "Fournier", "Gaudreault", "Gagne", "Garneau",
    "Gauthier", "Girard", "Gravel", "Gagnon", "Gagne", "Gerin", "Grenier", "Guerin", "Gosselin", "Guerin",
    "Dion", "Desjardins", "Desrochers", "Dufresne", "Fournier", "Girard", "Gauthier", "Gilbert", "Gosselin", "Lambert",
    "Lauzon", "Lefebvre", "Lemoine", "Levesque", "Michaud", "Morin", "Nadeau", "Perron", "Picard", "Plante",
    "Poirier", "Racine", "Remillard", "Rivard", "Rousseau", "Tremblay", "Vachon", "Veronique", "Villeneuve", "Valois",
    "Vallee", "Vallee", "Thibeault", "Trudeau", "Lafleur", "Lemoine", "Levesque", "Doucet", "Bissonnette", "Dube",
    "Delisle", "Deveau", "Jolicoeur", "Begin", "Beaulieu", "Belanger", "Boudreau", "Boucher", "Belanger", "Breton",
    "Brouillette", "Caron", "Castonguay", "Cavanagh", "Cloutier", "Collin", "Dupont", "Gagne", "Girard", "Gosselin",
    "Grenier", "Hebert", "Gauthier", "Robitaille", "Roy", "Simard", "Boutin", "Tremblay", "Thibault", "Vachon",
    "Brisson", "Gendron", "Brunet", "Gravel", "Vallee", "Girard", "Lafleur", "Laporte", "Marchand", "Gravel",
    "Harvey", "Michaud", "Chabot", "Coutu", "Bourgault", "Allaire", "Levesque", "Gagne", "Gosselin", "Plante",
    "Lemoine", "Chartrand"
}},
{CountryGenerator.Country.UnitedStates, new List<string> {
    "Adams", "Allen", "Anderson", "Baker", "Barnes", "Bell", "Bennett", "Black", "Blake", "Brooks",
    "Brown", "Bryant", "Campbell", "Cameron", "Carson", "Chapman", "Clark", "Cole", "Collins", "Cook",
    "Craig", "Curtis", "Davis", "Day", "Diaz", "Dixon", "Douglas", "Edwards", "Ellis", "Evans",
    "Ferguson", "Fisher", "Ford", "Franklin", "Freeman", "Garcia", "Gibson", "Gilbert", "Glover", "Graham",
    "Green", "Griffin", "Hall", "Harris", "Harrison", "Henderson", "Hill", "Hines", "Hughes", "Jackson",
    "James", "Jenkins", "Johnson", "Jones", "Jordan", "Kelley", "King", "Knight", "Lawrence", "Lee",
    "Lewis", "Long", "Martin", "Mason", "Matthews", "McCarthy", "McDonald", "Miller", "Moore", "Morris",
    "Murphy", "Nelson", "Newman", "Norris", "Palmer", "Parker", "Patterson", "Perez", "Phillips", "Price",
    "Ray", "Reed", "Reyes", "Richards", "Richardson", "Roberts", "Robinson", "Rodriguez", "Ross", "Russell",
    "Sanchez", "Scott", "Shaw", "Simpson", "Smith", "Snyder", "Stewart", "Stone", "Taylor", "Thomas",
    "Thompson", "Torres", "Turner", "Valdez", "Vasquez", "Walker", "Ward", "Washington", "Watson", "Webb",
    "White", "Williams", "Wilson", "Wood", "Woods", "Young", "Zimmerman", "Avery", "Barton", "Baxter",
    "Beasley", "Benton", "Bishop", "Blanchard", "Boone", "Bradley", "Brady", "Bray", "Bridges", "Bright",
    "Brooks", "Bryant", "Bullock", "Byers", "Cameron", "Campbell", "Carson", "Chavez", "Chavez", "Christensen",
    "Clark", "Clayton", "Clemens", "Cline", "Cobb", "Conner", "Cook", "Crawford", "Cross", "Curtis",
    "Dawson", "Decker", "Dennis", "Doyle", "Duncan", "Edwards", "Fletcher", "Ford", "Fowler", "Franks",
    "Garcia", "Garner", "Gates", "Gentry", "Gibson", "Goodman", "Graham", "Grayson", "Green", "Gregory",
    "Grove", "Hamilton", "Hancock", "Harper", "Harris", "Hayes", "Henderson", "Henson", "Herman", "Hickman",
    "Hill", "Holmes", "Howard", "Hudson", "Hunter", "James", "Jarvis", "Jensen", "Johnson", "Jones",
    "Jordan", "Joseph", "Keller", "Kendall", "Kennedy", "King", "Kirk", "Klein", "Kline", "Lambert",
    "Lamb", "Lawson", "Lee", "Leonard", "Lewis", "Lloyd", "Lynch", "Maddox", "Malone", "Mann",
    "Mason", "Mathis", "Miller", "Mills", "Mitchell", "Moore", "Morris", "Murphy", "Nash", "Nixon",
    "O'Connor", "Owens", "Parker", "Paul", "Pearson", "Perry", "Peters", "Peterson", "Pope", "Powers",
    "Pratt", "Price", "Reid", "Reyes", "Roberts", "Robertson", "Rodgers", "Rogers", "Ross", "Sampson",
    "Sanders", "Scott", "Shaw", "Sharp", "Shepherd", "Simpson", "Smith", "Snyder", "Stephens", "Stewart",
    "Stone", "Taylor", "Thompson", "Timmons", "Turner", "Walker", "Ward", "Watson", "Webster", "Weber",
    "Wells", "West", "White", "Woods", "Wyatt", "Yates"
}},
{CountryGenerator.Country.Sweden, new List<string> {
    "Andersson", "Johansson", "Karlsson", "Nilsson", "Eriksson", "Larsson", "Olsson", "Persson", "Svensson", "Gustafsson",
    "Lindstrom", "Jansson", "Pettersson", "Hansson", "Bergstrom", "Lundberg", "Martensson", "Fredriksson", "Bengtsson", "Lindqvist",
    "Wallin", "Hedlund", "Fransson", "Ahlberg", "Olofsson", "Jonsson", "Hakansson", "Lundqvist", "Rosenberg", "Lofgren",
    "Hellstrom", "Bjork", "Danielsson", "Eklund", "Berg", "Marklund", "Engstrom", "Akesson", "Bergman", "Kallstrom",
    "Dahlstrom", "Astrom", "Stenberg", "Gunnarsson", "Aberg", "Eliasson", "Stromberg", "Petersson", "Nordin", "Isaksson",
    "Sjostrom", "Forsberg", "Mellberg", "Hoglund", "Sjoberg", "Blomqvist", "Viklund", "Wikstrom", "Jonasson", "Friberg",
    "Jonsson", "Sjogren", "Rydberg", "Lofven", "Hakansson", "Nystrom", "Lindblom", "Almqvist", "Rydahl", "Akerman",
    "Bjorklund", "Gunnarsson", "Moller", "Sandstrom", "Strom", "Backlund", "Axelsson", "Hansson", "Lind", "Bohlin",
    "Bengt", "Tornqvist", "Axel", "Knutsson", "Carlsson", "Gustaf", "Sundberg", "Hedberg", "Soderberg", "Vallgren",
    "Holm", "Hult", "Linder", "Wahlstrom", "Stahl", "Rosen", "Berglund", "Johan", "Martensson", "Norberg",
    "Karlsson", "Aman", "Lundin", "Sundin", "Heinonen", "Hansson", "Petersson", "Kjellstrom", "Widell", "Backman",
    "Johansson", "Oberg", "Mansson", "Hagg", "Norstrom", "Thorn", "Fredrik", "Friberg", "Hassel", "Hjerpe",
    "Norlin", "Vesterberg", "Berger", "Bergstrom", "Anders", "Axel", "Ehrnberg", "Falk", "Bjorkman", "Bynell",
    "Axel", "Ferm", "Holmqvist", "Kronsjo", "Lindqvist", "Norstrom", "Wahlberg", "Dahl", "Svensson", "Vag",
    "Peters", "Norrman", "Zetterberg", "Gyllen", "Jonsson", "Stig", "Wiklund", "Fors", "Lundqvist", "Hakansson",
    "Jacobsson", "Jager", "Persson", "Svensson", "Frojd", "Gronlund", "Rudholm", "Lundh", "Blom", "Wilhelmsson",
    "Palsson", "Kallstrom", "Hedstrom", "Lof", "Alm", "Lundell", "Hammar", "Ekvall", "Mattsson", "Lowen",
    "Hojer", "Karl", "Gronberg", "Sund", "Stenberg", "Frojd", "Sjowall", "Schwede", "Jonsson", "Gustafsson"
}},
{CountryGenerator.Country.Russia, new List<string> {
    "Ivanov", "Petrov", "Sidorov", "Smirnov", "Kuznetsov", "Popov", "Vasiliev", "Mikhailov", "Novikov", "Fedorov",
    "Morozov", "Volkov", "Vernikov", "Andreev", "Zaytsev", "Dmitriev", "Pavlov", "Nikolaev", "Romanov", "Stepanov",
    "Baranov", "Chernov", "Orlov", "Belyaev", "Lebedev", "Borisov", "Semenov", "Tikhonov", "Karpov", "Golubev",
    "Kovalev", "Yakubov", "Kovalev", "Gusev", "Sukhov", "Alekseev", "Gavrilov", "Shishkin", "Ilyin", "Frolov",
    "Krasnov", "Kuzin", "Sviridov", "Glushkov", "Frolov", "Solovyov", "Klimov", "Vorobyov", "Tkachenko", "Shirokov",
    "Maksimov", "Sharikov", "Yakovlev", "Antonenko", "Ponomarev", "Semenov", "Kravtsov", "Krutov", "Abrikosov", "Anisimov",
    "Kovalenko", "Osipov", "Rudakov", "Kovalenko", "Pilipenko", "Kostin", "Ivanenko", "Syrkov", "Novikov", "Donskoy",
    "Kharlamov", "Gerasimov", "Bulgakov", "Belov", "Cherepanov", "Kolbaskov", "Cherkashin", "Frolov", "Gavrilov", "Skvortsov",
    "Golovachev", "Alekseev", "Chirkov", "Sukharev", "Lukyanov", "Karpov", "Sergeyev", "Belyakov", "Zakharov", "Kokorin",
    "Goryachev", "Maslov", "Ivankov", "Shorin", "Kokorin", "Vishnevsky", "Krivtsov", "Stefanov", "Kozlov", "Kosenko",
    "Koretsky", "Vostrikov", "Cheremisin", "Mishchenko", "Novik", "Frolov", "Demidov", "Barchuk", "Slavkin", "Yegorov",
    "Serebryakov", "Shubin", "Fedorov", "Kostroma", "Zaharov", "Kulikov", "Yermolov", "Kulikov", "Sizov", "Solovyev",
    "Ryzhov", "Frolov", "Levchenko", "Chernov", "Lazarev", "Burov", "Matveev", "Sukhorukov", "Semenov", "Korneev",
    "Abdulin", "Serdyukov", "Kotov", "Zaycev", "Maslov", "Grigoryev", "Sokolov", "Samsonov", "Alexandrov", "Pryanikov",
    "Mikhalchuk", "Babanin", "Marin", "Dudkin", "Panteleev", "Rostovtsev", "Penkov", "Solovey", "Chervyakov", "Terekhov",
    "Gurevich", "Kovalevsky", "Frolov", "Vasiliev", "Frolov", "Zhitkov", "Shumov", "Makarov", "Zakharov", "Korolev",
    "Fedotov", "Sharapov", "Trofimov", "Krutov", "Viktorov", "Pavlov", "Gorodetsky", "Tikhomirov", "Tsyganov", "Aksenov",
    "Samsonov", "Zotov", "Vasilyev", "Mikheyev", "Shcherbakov", "Abakumov", "Savin", "Ulyanov", "Polikov", "Nikitin",
    "Romanov", "Gritsuk", "Surov", "Solovyev", "Karimov", "Tarkhov", "Aleshin", "Klyuchnikov", "Martynov", "Sokolov",
    "Akhmetov", "Yakovlev", "Gorin", "Taranov", "Shubov", "Lev", "Rodionov", "Pozdnyakov", "Frolov", "Gornov"
}},
{CountryGenerator.Country.Finland, new List<string> {
    "Korhonen", "Virtanen", "Nieminen", "Makela", "Hamalainen", "Jarvinen", "Lahtinen", "Ritola", "Laitinen", "Lehtonen",
    "Kallio", "Peltola", "Salminen", "Heikkinen", "Jokinen", "Saarinen", "Tuominen", "Karjalainen", "Hakala", "Lehtimaki",
    "Rasanen", "Takala", "Aho", "Vaisanen", "Koskinen", "Salo", "Miettinen", "Hukkanen", "Niskanen", "Rinne",
    "Lehtola", "Koskinen", "Ollila", "Uusitalo", "Alatalo", "Linnaluoto", "Manninen", "Sundstrom", "Rajala", "Vahaaho",
    "Koskela", "Vainio", "Rantanen", "Tirkkonen", "Eriksson", "Laitila", "Timonen", "Ranta", "Koivisto", "Lepisto",
    "Pasanen", "Kallioinen", "Piirainen", "Aalto", "Sillanpaa", "Kivinen", "Yli-Paavola", "Korpela", "Koskelainen", "Rinta",
    "Koskinen", "Manner", "Pyykko", "Husso", "Heikkila", "Pakarinen", "Heikkinen", "Hamalainen", "Aaltonen", "Pasanen",
    "Kauppinen", "Mikkola", "Palo", "Vanska", "Ronkainen", "Vepsalainen", "Alanko", "Vaananen", "Raisanen", "Harkala",
    "Uusimaki", "Tuomisto", "Karvinen", "Kallio", "Tormanen", "Halla", "Loponen", "Heikkila", "Sundqvist", "Lonnqvist",
    "Lehtimaki", "Lahde", "Juntunen", "Salmi", "Kilpelainen", "Vaananen", "Sundstrom", "Kinnunen", "Lahtela", "Koskinen",
    "Aaltio", "Rissanen", "Raunio", "Leinonen", "Kettunen", "Torma", "Vuorela", "Vaananen", "Karkkainen", "Vihinen",
    "Tahka", "Makinen", "Timonen", "Hanninen", "Heikkila", "Pohjonen", "Lindstrom", "Koski", "Puranen", "Koskelainen",
    "Lappalainen", "Myllyla", "Niemi", "Lindholm", "Hyvarinen", "Hautala", "Hautamaki", "Martikainen", "Juvonen", "Niemela",
    "Tervonen", "Koskelainen", "Pohjonen", "Palosaari", "Vikstrom", "Hakulinen", "Rinne", "Valimaa", "Tolvanen", "Huuskonen",
    "Miettinen", "Pakkanen", "Saraste", "Raisanen", "Alaparta", "Turunen", "Kinnunen", "Vasama", "Metsala", "Aaltonen",
    "Pasanen", "Linnakoski", "Lahteenmaki", "Heikkila", "Lappalainen", "Saloranta", "Niukkanen", "Ruotsalainen", "Koskinen",
    "Kiviniemi", "Koivuniemi", "Kallio", "Rokkanen", "Hamalainen", "Lahde", "Makitalo", "Kivilahti", "Lappalainen", "Aalto",
    "Rokka", "Tarkkala", "Makela", "Vaisanen", "Pohjalainen", "Karjalainen", "Tarkka", "Vaisanen", "Kauppinen", "Makela",
    "Kontio", "Tuomela", "Vaha", "Heikkinen", "Saarela", "Vaananen", "Ikonen", "Hartikainen", "Peltola", "Koskelainen",
    "Rantasalo", "Alatalo", "Sallinen", "Vihola", "Jalkanen", "Hujanen", "Kiiskinen", "Jantti", "Salo", "Makinen",
    "Torpainen", "Kumpulainen", "Kallio", "Vahakangas", "Iiskola", "Vaisanen", "Ahlberg", "Pohjalainen", "Vihinen", "Vaisanen"
}},
{CountryGenerator.Country.CzechRepublic, new List<string> {
    "Novak", "Svoboda", "Novotny", "Dvorak", "Cerny", "Prochazka", "Kovar", "Kucera", "Jelinek", "Horak",
    "Moravec", "Kopecky", "Simko", "Pavlicek", "Vesely", "Pokorny", "Fiala", "Bartos", "Zeman", "Petr",
    "Marek", "Maly", "Ruzicka", "Stastny", "Havlicek", "Kral", "Kriz", "Benes", "Kubicek", "Cech",
    "Rychly", "Tesar", "Lukas", "Urban", "Jiranek", "Janda", "Jelinek", "Krizek", "Muller", "Ruzicka",
    "Blazek", "Hladik", "Filip", "Stehlik", "Kucera", "Cerny", "Pavlik", "Votava", "Vagner", "Sefcu",
    "Kovar", "Pokorny", "Vavra", "Klimek", "Zelenka", "Bednar", "Kocourek", "Prusa", "Landa", "Vesely",
    "Vit", "Linhart", "Kosek", "Smid", "Cermak", "Bohac", "Bartosek", "Kocour", "Polak", "Vosecek",
    "Tichy", "Ruzicka", "Starka", "Rudolf", "Sebek", "Holecek", "Smetana", "Riha", "Kalvoda", "Dolezal",
    "Kolar", "Cejnar", "Maly", "Tuma", "Tomek", "Kubik", "Zavrel", "Pavlicek", "Mikulas", "Horvath",
    "Fiser", "Macek", "Zahradnik", "Hosek", "Mihulka", "Filip", "Malek", "Kriz", "Truhlar", "Martinek",
    "Jedlicka", "Bacik", "Volek", "Vitek", "Vana", "Pavlovsky", "Kopecky", "Hradek", "Kucera", "Malek",
    "Kubicek", "Havrda", "Babka", "Tomes", "Elias", "Stehlik", "Pechacek", "Hradek", "Vavra", "Kolar",
    "Ladman", "Tuma", "Hrdina", "Zelenka", "Kus", "Baier", "Berg", "Kucera", "Vlacil", "Nemec",
    "Kucera", "Rehak", "Krizek", "Stepanek", "Rehak", "Jirka", "Svejdik", "Gajdusek", "Smutny", "Pluhar",
    "Pohl", "Soucek", "Jelinek", "Vavra", "Pekarek", "Kaspar", "Jurecka", "Kratky", "Rokos", "Maly",
    "Blazek", "Kocian", "Bujno", "Beranek", "Prochazka", "Zeman", "Hajn", "Rous", "Sandera", "Moravec",
    "Fiala", "Pekar", "Kovar", "Hradek", "Moc", "Brezina", "Reznicek", "Kolek", "Bures", "Horak",
    "Nemec", "Vesely", "Pavlik", "Kus", "Kubik", "Knejzik", "Bily", "Hudecek", "Vacek", "Dolezal",
    "Prochazka", "Sevcik", "Svoboda", "Kubes", "Pavelka", "Rohac", "Vojta", "Vechet", "Hofmann", "Klesla",
    "Svoboda", "Skalicky", "Stepan", "Plechacek", "Safarik", "Hanzlik", "Lukas", "Andel", "Cibulka", "Kana",
    "Bocek", "Knecht", "Hrdy", "Travnicek", "Zoufaly", "Soucek", "Zizka", "Rybka", "Marek", "Mottl",
    "Lnenicka", "Kozak", "Kriz", "Kolar", "Stejskal", "Kucera", "Kriz", "Vaclav", "Becvar", "Dvorak",
    "Soukal", "Lind", "Penicka", "Parez", "Krecek", "Jilek", "Charvat", "Bodlak", "Hospodka", "Muller",
    "Povey", "Hroch", "Domasin", "Sindelar", "Tuma", "Babek", "Kubacek", "Sulc", "Ruzicka", "Kadlec"
}},
{CountryGenerator.Country.Slovakia, new List<string> {
    "Novak", "Horvath", "Kovac", "Toth", "Varga", "Marek", "Lukac", "Petrik", "Mihal", "Kuzma",
    "Ruzicka", "Sladek", "Fico", "Kovacik", "Gabor", "Bakos", "Stastny", "Juhas", "Andrej", "Bielik",
    "Simko", "Bartos", "Buzas", "Mikulec", "Cintula", "Stefan", "Bartos", "Krainka", "Valent", "Kozak",
    "Mocna", "Pavol", "Kubik", "Sebik", "Hrncar", "Baran", "Kolar", "Vojta", "Medved", "Kis",
    "Jankovic", "Varga", "Zeman", "Lajcak", "Sukennik", "Kocan", "Kosican", "Lukas", "Sykora", "Fedor",
    "Rozsa", "Pech", "Gorib", "Viliam", "Lefcik", "Cizner", "Tomek", "Oremus", "Vargova", "Dobos",
    "Kubis", "Buchta", "Ferencik", "Linder", "Tursky", "Farkas", "Sokoli", "Saga", "Surovy", "Kosmak",
    "Svetlik", "Hruska", "Kriz", "Saga", "Lajcakova", "Stefanik", "Kovacova", "Kubikova", "Hric",
    "Hajduk", "Beno", "Cibak", "Tomasov", "Rakoczi", "Vojtko", "Horecky", "Kolesar", "Hasiak", "Baranek",
    "Tomas", "Sokol", "Kmet", "Vrabel", "Macek", "Liptak", "Polak", "Kozuch", "Havrila", "Tothova",
    "Simkova", "Regec", "Nagy", "Cernak", "Zeman", "Liska", "Mraz", "Lani", "Kukucka", "Andrassy",
    "Kolesar", "Kovacik", "Zahradnik", "Cisar", "Feldmann", "Kuban", "Aspald", "Kubickova", "Tvrdon",
    "Sadecky", "Matus", "Kromka", "Petrik", "Nemeth", "Slobodnik", "Matuska", "Maros", "Greben",
    "Racko", "Lukasova", "Blahuta", "Fleming", "Susanka", "Tihanyi", "Hramina", "Sojka", "Kafun",
    "Zimna", "Novotny", "Kalina", "Kubicek", "Rehak", "Kokos", "Bojnik", "Petras", "Fazekas", "Erik",
    "Kovacikova", "Paskat", "Uher", "Cepan", "Gaborova", "Tothova", "Hlava", "Gregor", "Huzovic", "Bencik",
    "Kuzmany", "Reiser", "Dudas", "Holbik", "Kuzma", "Fabek", "Bojda", "Hodas", "Kopcak", "Erdelyi",
    "Nadasky", "Racko", "Kozak", "Bral", "Vrabel", "Dudasova", "Stepanik", "Slusny", "Jancik", "Babiak",
    "Sveda", "Hlava", "Hola", "Tothova", "Martincova", "Beckova", "Hanzel", "Skoda", "Kovac", "Kovacik",
    "Lunter", "Koch", "Zajac", "Toth", "Kusy", "Marecek", "Gabris", "Kalinak", "Hron", "Limpach", "Mihal",
    "Stefanec", "Urban", "Fraj", "Mesarcik", "Hojnik", "Petrovic", "Novotny", "Kozma", "Sykora", "Jurenka",
    "Zubak", "Kolenik", "Bodnar", "Kaluza", "Klein", "Sperka", "Siroky", "Pekar", "Bielik", "Geci", "Jasova",
    "Novak", "Sukennik", "Svoboda", "Brix", "Rasky", "Petrasek", "Balko", "Macko", "Biedermann", "Tomas",
    "Zubak", "Jano", "Toth", "Hrusovsky", "Lazar", "Sebik", "Rohac", "Capek", "Rybar", "Hofmann", "Kender",
    "Palicka", "Vano", "Sestak", "Choma", "Nemec", "Valo", "Smolik", "Benes", "Pokorny", "Horvathova",
    "Kral", "Zahradnik", "Brat", "Mikulas", "Feri", "Zubiak", "Nagy", "Matus", "Kralik", "Sovak"
}},
{CountryGenerator.Country.Switzerland, new List<string> {
    "Muller", "Meier", "Schmidt", "Bauer", "Keller", "Weber", "Lerch", "Zimmermann", "Schneider", "Huber",
    "Fischer", "Perrin", "Richter", "Bucher", "Gmur", "Gerber", "Kaufmann", "Graf", "Jager", "Lang",
    "Steiner", "Hug", "Haas", "Berger", "Kunz", "Zollinger", "Luscher", "Suter", "Notzli", "Moser",
    "Scherer", "Ziegler", "Gasser", "Wyss", "Frei", "Hafliger", "Buhler", "Roth", "Durrer", "Wenger",
    "Schmid", "Bacher", "Kaufmann", "Tanner", "Aeschlimann", "Vogel", "Wick", "Jost", "Hoffmann", "Glauser",
    "Reinhardt", "Buhler", "Moser", "Muller", "Keller", "Haller", "Loffel", "Zenger", "Meyer", "Ziegler",
    "Kost", "Schwarz", "Amstad", "Schnyder", "Gisler", "Erni", "Kistler", "Kropf", "Bernet", "Ammann",
    "Albrecht", "Mausli", "Luscher", "Bircher", "Guggenbuhl", "Spring", "Buchli", "Frick", "Widmer", "Baumann",
    "Ruegg", "Suess", "Thalmann", "Munch", "Heiniger", "Stampfli", "Strickler", "Lutz", "Krause", "Hirschi",
    "Sauter", "Vollmer", "Bolliger", "Luchsinger", "Perren", "Hasler", "Durr", "Koch", "Benz", "Bieri",
    "Lehmann", "Bergmann", "Tschudin", "Schellenberg", "Bernet", "Schumacher", "Oberli", "Flury", "Zurcher", "Hunziker",
    "Muggli", "Stalder", "Furer", "Butikofer", "Brock", "Knecht", "Stucki", "Schurter", "Kocher", "Raemy",
    "Schaub", "Scharf", "Wittwer", "Hanggi", "Haller", "Meyer", "Gahwiler", "Truniger", "Basler", "Pax",
    "Rieder", "Burkhard", "Moser", "Eggli", "Kumli", "Keller", "Sumi", "Trueb", "Wuthrich", "Beyer",
    "Bader", "Vonarburg", "Gubler", "Bachi", "Flury", "Geiser", "Kappeli", "Sutter", "Ammann", "Graber",
    "Staub", "Fankhauser", "Peter", "Arn", "Muller", "Jenkins", "Niederer", "Schwabe", "Schuler", "Zehnder",
    "Fassbind", "Zulliger", "Kleiber", "Deuber", "Cavelti", "Schiesser", "Fritsch", "Bieli", "Spuhler", "Herrmann",
    "Marti", "Bachmann", "Gisi", "Jermann", "Egli", "Weber", "Krahenbuhl", "Scherrer", "Zahner", "Truttmann",
    "Hohl", "Senn", "Fetzer", "Ernst", "Kramer", "Will", "Hefti", "Gerrits", "Pluss", "Seiler",
    "Boll", "Bach", "Gurke", "Steiner", "Muller", "Schwartz", "Hasler", "Muller", "Frei", "Keller",
    "Staub", "Sigg", "Bacchi", "Burger", "Frei", "Steiner", "Bohren", "Gantenbein", "Diener", "Herger",
    "Luchsinger", "Giger", "Lotscher", "Hochstrasser", "Haussmann", "Wolf", "Pfeiffer", "Kuhn", "Zimmerli",
    "Rolf", "Blatter", "Koller", "Ditter", "Schilliger", "Wolf", "Henn", "Suter", "Knecht", "Banz",
    "Tobler", "Jost", "Erb", "Huber", "Chedel", "Weingart", "Zingg", "Mori", "Luginbuhl", "Muller",
    "Riegg", "Vogt", "Schonenberger", "Zimmerman", "Brunner", "Basler", "Ackermann", "Kappeler", "Weber",
    "Blaettler", "Zuber", "Buhrer", "Brem", "Murer", "Scherer", "Pankow", "Ziegler", "Rohrer", "Bischof",
    "Junemann", "Biel", "Stockli", "Guntensperger", "Eberle", "Zurcher", "Bolin", "Meier", "Rapp", "Walser",
    "Hirzel", "Meier", "Schweizer", "Mohl", "Sporri", "Sauer", "Scheck", "Auer", "Muller", "Lowen",
    "Scheidegger", "Pittet", "Hug", "Bortoluzzi", "Reiner", "Sigrist", "Carigiet", "Gasser", "Suter", "Kessler"
}},
{CountryGenerator.Country.Germany, new List<string> {
    "Muller", "Schmidt", "Schneider", "Fischer", "Weber", "Meyer", "Wagner", "Becker", "Hoffmann", "Schafer",
    "Bauer", "Richter", "Koch", "Zimmermann", "Schuster", "Klein", "Wolf", "Braun", "Schulz", "Hofmann",
    "Lange", "Schmitt", "Werner", "Hartmann", "Lutz", "Maier", "Krause", "Jager", "Bohm", "Lorenz",
    "Schumacher", "Scholz", "Neumann", "Zimmer", "Beyer", "Seidel", "Peters", "Jung", "Schreiber", "Mayer",
    "Weiss", "Kaiser", "Haas", "Lang", "Weber", "Weidmann", "Sauer", "Roth", "Vogel", "Pohl",
    "Kuhn", "Scholz", "Hahn", "Friedrich", "Berger", "Keller", "Walter", "Petersen", "Sachs", "Blick",
    "Ziegler", "Gunther", "Jansen", "Schafer", "Eckert", "Roder", "Zimmerer", "Reuter", "Bock", "Fink",
    "Schwarz", "Richter", "Liebig", "Herrmann", "Schwabe", "Wehr", "Ruppert", "Schick", "Frohlich", "Muller",
    "Freund", "Meier", "Rupp", "Stark", "Voss", "Senger", "Rieck", "Zeller", "Niedermeier", "Pape",
    "Dieter", "Kaiser", "Steger", "Jost", "Moser", "Borner", "Heinrich", "Bremer", "Wegner", "Moller",
    "Oster", "Rieger", "Heinz", "Schaller", "Buchholz", "Fahrner", "Rosenberg", "Fiedler", "Brunner", "Haas",
    "Heinrich", "Hoffmann", "Stein", "Neidhardt", "Marx", "Brandt", "Hoffmeister", "Kruger", "Sommer", "Richter",
    "Bauer", "Geiger", "Sommers", "Kraft", "Keller", "Kraft", "Bonisch", "Ehrlich", "Schick", "Walther",
    "Bender", "Klein", "Rath", "Scherer", "Kirchhoff", "Scholz", "Meissner", "Gotz", "Doring", "Moch",
    "Fischer", "Stein", "Thiele", "Schubert", "Lowe", "Franz", "Harte", "Schwalbe", "Henn", "Junker",
    "Wiesner", "Kraft", "Pohl", "Lohr", "Richter", "Scholz", "Pohl", "Brunner", "Greiner", "Strauch",
    "Henkel", "Lucke", "Muller", "Markus", "Gunter", "Goetz", "Hulsmann", "Hennig", "Reimann", "Wagner",
    "Siefert", "Volk", "Bar", "Erhardt", "Ruppert", "Scholz", "Kratzer", "Geisler", "Berger", "Gert",
    "Bach", "Schlegel", "Kraft", "Franke", "Schwager", "Harter", "Wegener", "Hahnel", "Halter", "Huppauf",
    "Brand", "Lehmann", "Weiss", "Schroder", "Stange", "Bock", "Adler", "Kempf", "Albrecht", "Klein",
    "Hensel", "Schick", "Bartel", "Kaiser", "Heber", "Strack", "Seifert", "Steiger", "Schreiber", "Tonnies",
    "Gerhardt", "Weber", "Hartwig", "Schild", "Krebs", "Schwan", "Link", "Schneider", "Schon", "Muller",
    "Voss", "Sachs", "Hennig", "Zobel", "Fendt", "Lang", "Brugger", "Brunner", "Schwalm", "Beyermann",
    "Lehmann", "Beyer", "Schwager", "Muller", "Ziegler", "Preuss", "Roth", "Gotz", "Forster", "Wettstein",
    "Sturm", "Wiesemann", "Heller", "Zepter", "Hartung", "Siefert", "Baumann", "Forster", "Vogt", "Rieger",
    "Weis", "Haller", "Riemer", "Schwalm", "Kroger", "Heil", "Schreiber", "Abt", "Holz", "Bernhard",
    "Arndt", "Peters", "Weiss", "Hufner", "Fischer", "Schweitzer", "Herwig", "Schafer", "Steger", "Ober",
    "Thiele", "Heintz", "Engel", "Hock", "Pohl", "Goebel", "Kessler", "Heim", "Lenz", "Kappel", "Lorenz"
}},
{CountryGenerator.Country.Denmark, new List<string> {
    "Andersen", "Nielsen", "Jensen", "Hansen", "Pedersen", "Larsen", "Christensen", "Rasmussen", "Sorensen", "Petersen",
    "Madsen", "Poulsen", "Jorgensen", "Eriksen", "Karlsson", "Olsen", "Thomsen", "Iversen", "Bendt", "Vestergaard",
    "Bech", "Johansen", "Mortensen", "Haug", "Friis", "Bakker", "Lind", "Mathiasen", "Lund", "Vang",
    "Roth", "Skaarup", "Skov", "Simonsen", "Henningsen", "Berg", "Bjerg", "Rasmussen", "Kjaer", "Holm",
    "Kirk", "Knudsen", "Vester", "Brammer", "Kjeldsen", "Skytte", "Krogh", "Andersen", "Wichmann", "Nymann",
    "Mikkelsen", "Aagaard", "Hvid", "Berggren", "Hansen", "Skaarup", "Rydberg", "Gulliksen", "Rasmussen", "Petersen",
    "Christiansen", "Skovgaard", "Munch", "Sogaard", "Dalgaard", "Sorensen", "Muller", "Lund", "Skarp", "Hakon",
    "Vik", "Mikkelsen", "Christensen", "Vandborg", "Aarhus", "Bakke", "Borgholm", "Ziegler", "Lindholm", "Skovlund",
    "Mikkelson", "Boesen", "Pedersen", "Friedrichsen", "Kristensen", "Lindegaard", "Nygaard", "Andersson", "Kjaergaard", "Nielsen",
    "Olesen", "Skaarup", "Knudsen", "Solv", "Bjerre", "Hansen", "Sorensen", "Schmidt", "Hjorth", "Smed",
    "Bjerregaard", "Jacobsen", "Larsen", "Gulbrandsen", "Vester", "Brondum", "Fischer", "Knudsen", "Eklund", "Hansen",
    "Egeberg", "Borring", "Dal", "Bakker", "Moll", "Ljung", "Jorgensen", "Bistrup", "Helgesen", "Skelton",
    "Skaarup", "Host", "Hoj", "Henriksen", "Kruse", "Brenner", "Bach", "Boe", "Glud", "Jensen",
    "Lundberg", "Karlsen", "Soreide", "Skaar", "Feldberg", "Larsen", "Poul", "Brodersen", "Stavros", "Haugstad",
    "Hansen", "Skjold", "Hojgaard", "Hjelm", "Tegner", "Fuglsang", "Jorgensen", "Andersen", "Madsen", "Nielsen",
    "Brink", "Bakker", "Thygesen", "Hansen", "Mork", "Tornoe", "Damgaard", "Borg", "Dahl", "Nygaard",
    "Simonsen", "Bjerg", "Skaarup", "Norgaard", "Skog", "Jorgensen", "Floe", "Schmidt", "Kristensen", "Taekker",
    "Arnesen", "Brandt", "Hoffmann", "Jacobsen", "Jensen", "Jorgensen", "Petersen", "Skaarup", "Hansen", "Skov",
    "Rasmussen", "Nielsen", "Hansen", "Madsen", "Christensen", "Sorensen", "Pedersen", "Nielsen", "Knudsen", "Jacobsen",
    "Tegner", "Schou", "Thygesen", "Dahl", "Lund", "Vang", "Skov", "Eriksen", "Bendtsen", "Holm",
    "Munch", "Petersen", "Christensen", "Skov", "Bakker", "Lind", "Madsen", "Simonsen", "Jensen", "Knudsen",
    "Gron", "Heiberg", "Hansen", "Haug", "Sorensen", "Lund", "Jorgensen", "Mikkelsen", "Lund", "Skaarup",
    "Kjeldsen", "Bendtsen", "Bakker", "Skaarup", "Aagaard", "Jensen", "Pedersen", "Christensen", "Mortensen", "Knudsen",
    "Lind", "Madsen", "Skovgaard", "Taekker", "Bengtsson", "Andersson", "Vestergaard", "Simonsen", "Nielsen", "Lindegaard",
    "Hansen", "Andersen", "Bjerre", "Wang", "Petersen", "Madsen", "Lund", "Erlang", "Thomsen", "Dahl",
    "Bakker", "Jorgensen", "Kristensen", "Hansen", "Poul", "Iversen", "Muller", "Skaarup", "Frost", "Bakker",
    "Lund", "Jacobsen", "Thomsen", "Thygesen", "Jorgensen", "Sogaard", "Vestergaard", "Vester", "Korsholm", "Hansen"
}},
{CountryGenerator.Country.Latvia, new List<string> {
    "Andersons", "Berzins", "Cakste", "Dzerins", "Eglitis", "Gailitis", "Grinbergs", "Ilzins", "Jekabsons", "Klavins",
    "Kovals", "Lapins", "Lazdins", "Liepins", "Lusis", "Meiers", "Mierins", "Miksons", "Ozols", "Petersons",
    "Priedite", "Rudzitis", "Silis", "Skruzitis", "Smuteks", "Strautins", "Strode", "Toms", "Upenieks",
    "Zarins", "Ziedonis", "Blazevics", "Lazarevs", "Jekabs", "Vasilevs", "Rimsevics", "Balkis", "Berkis",
    "Vitols", "Upenieks", "Veckalnins", "Berzina", "Kokins", "Lidaka", "Grinberga", "Zarins", "Kalnins",
    "Karpovs", "Vasiljevs", "Berzins", "Gailitis", "Grisins", "Dainis", "Lusis", "Dulevics", "Milenbergs",
    "Rubenis", "Zvaigzne", "Klavina", "Toms", "Riekstin", "Berzins", "Eilands", "Luksevics", "Berzins", "Vasiljevs",
    "Brencis", "Berzins", "Riekstin", "Kalnins", "Racenis", "Cipruss", "Selickis", "Zukovs", "Iljins",
    "Mezs", "Paegle", "Reinis", "Skrastins", "Vecvagars", "Rivups", "Berzins", "Skrastins", "Mikasevics",
    "Silis", "Lapins", "Zarins", "Skadins", "Lazdins", "Krumins", "Salmanis", "Balkis", "Berzins",
    "Mezins", "Prieditis", "Jurgens", "Rudzitis", "Smits", "Rudans", "Skruzitis", "Rusins", "Mikelsons",
    "Lacis", "Strazds", "Strelis", "Berzs", "Niedra", "Oskars", "Varda", "Dzintars", "Kalnina", "Ziedonis",
    "Talbergs", "Vigants", "Rudzitis", "Ziedins", "Krumins", "Jekabsons", "Kalnins", "Raitis", "Vasilevs",
    "Skruzitis", "Vitols", "Redlihs", "Rudzitis", "Mezs", "Celajs", "Klavins", "Vitols", "Berzs", "Berzins",
    "Vele", "Spalvins", "Grigans", "Kaitans", "Jansons", "Licis", "Pavlovs", "Berzins", "Ciriks", "Puce",
    "Krumins", "Mednis", "Matisons", "Mikelsons", "Berzins", "Karaskovs", "Skrastins", "Klotins", "Vitols",
    "Berzs", "Zarins", "Blugers", "Petersone", "Racenis", "Mezs", "Rimsevics", "Lacplesis", "Mikalis",
    "Kalnins", "Raudive", "Mikelis", "Pucins", "Tile", "Paegle", "Indrikis", "Piedel", "Berzins", "Ziedins",
    "Dzeferis", "Skruzitis", "Bruzis", "Berzins", "Gailitis", "Strode", "Strelis", "Grigans", "Jekabsons",
    "Andersen", "Jekabs", "Prieditis", "Ziedonis", "Jansons", "Grisins", "Klavins", "Licis", "Mikelis", "Kalnins",
    "Petersone", "Zakis", "Berzis", "Priedite", "Lazdins", "Balkis", "Strelis", "Feldmanis", "Straupe", "Rubenis",
    "Berzins", "Skele", "Rudzitis", "Lazdins", "Petersons", "Blazevics", "Birzgalis", "Miller", "Plume", "Klavs",
    "Straupe", "Skruzitis", "Rudzitis", "Berzins", "Raudive", "Zarins", "Berzins", "Dzerins", "Mars",
    "Grinbergs", "Vacietis", "Aleksejevs", "Lazarevs", "Petersons", "Balkis", "Blazevics", "Kalnins", "Ziedonis"
}},
{CountryGenerator.Country.Belarus, new List<string> {
    "Abrosimov", "Akimov", "Alferov", "Andreev", "Antonov", "Arkhipov", "Babich", "Baklanov", "Baranov", "Bashirov",
    "Belov", "Biryukov", "Bogatov", "Bolotov", "Bondarenko", "Borodin", "Bulatov", "Chernov", "Chizh", "Churkin",
    "Dmitriev", "Doroshenko", "Efimov", "Fedorov", "Frolov", "Gavrilov", "Golubev", "Gorbatov", "Gorbunov", "Gritsenko",
    "Grishin", "Ilyin", "Ivanov", "Kalinov", "Karpov", "Kolesnikov", "Kornilov", "Korolev", "Koshelev", "Kovalev",
    "Kozlov", "Kuznetsov", "Lebedev", "Leonov", "Lisov", "Lobov", "Lukyanov", "Malyshev", "Markov", "Matveev",
    "Melnikov", "Mikhailov", "Morozov", "Nikiforov", "Nikulin", "Novikov", "Ozerov", "Pavlov", "Petrov", "Piskunov",
    "Polyakov", "Pukinel", "Rakov", "Romanov", "Ryabov", "Semenov", "Sidorov", "Sorokin", "Stepanov", "Tikhonov",
    "Timoshenko", "Ushakov", "Vasiliev", "Vasilenko", "Vengerov", "Vikhrov", "Vinogradov", "Volkov", "Vorobiev", "Yakovlev",
    "Zaharov", "Zheleznov", "Zhukov", "Zinovyev", "Abrikosov", "Aliev", "Alyokhin", "Antonyuk", "Artemiev", "Bashir",
    "Belyakov", "Berestov", "Blagov", "Bondar", "Borisenko", "Chichkanov", "Cherkesov", "Chernyavsky", "Chervonov", "Chesnokov",
    "Chikalo", "Dikarev", "Dolin", "Dovzhenko", "Egoshin", "Frolova", "Gavrikov", "Gorbun", "Gravtsov", "Hochkin",
    "Ivanchenko", "Kazakov", "Kirichenko", "Kiselyov", "Kleshchev", "Klevtsov", "Kolesnik", "Korneev", "Krasikov", "Kudryashov",
    "Kulikov", "Lebedev", "Lisovsky", "Lysenko", "Makarov", "Malikov", "Mikhalev", "Moroz", "Nevzorov", "Nikolaev",
    "Novoselov", "Orlov", "Pankov", "Pavlik", "Petrovich", "Pugachov", "Razumov", "Rodionov", "Rukavishnikov", "Ryzhov",
    "Savyuk", "Sikorsky", "Sidorenko", "Sinitsa", "Sivov", "Sorokov", "Stukalov", "Tatarinov", "Terebov", "Tolstov",
    "Trofimov", "Tyulkin", "Ustyugov", "Vekshin", "Vishnevsky", "Vladimirov", "Volkovich", "Voronov", "Vysotsky", "Zadvornov",
    "Zaharchenko", "Zhukovski", "Ziminyuk", "Arefyev", "Borisov", "Budayev", "Burkov", "Chalov", "Cherezov", "Cherenov",
    "Chukov", "Daskalov", "Drobyshev", "Dubrov", "Fedorova", "Frolova", "Gorbenko", "Gorodetsky", "Grozov", "Gribov",
    "Gushchin", "Ilyich", "Kalinchuk", "Karpushin", "Kireev", "Kirillov", "Klementyev", "Klimov", "Kovtun", "Korolevsky",
    "Kuzmin", "Lukach", "Lazarev", "Lishchinsky", "Malinovsky", "Markovskij", "Matviev", "Melikhov", "Mikheyev", "Molchanov",
    "Morozovich", "Nazarov", "Nechaeva", "Nevstruev", "Nikitenko", "Novikovskij", "Olhovski", "Pavlichenko", "Perov", "Platonov",
    "Poltavsky", "Rakhmanov", "Rogov", "Romanovich", "Rozov", "Sazonov", "Sizov", "Slepov", "Sokolenko", "Sukharev",
    "Ternovoy", "Titov", "Tkachov", "Vasilenko", "Vasiliev", "Vinnikov", "Vladov", "Vdovichenko", "Vilkova", "Yuriev",
    "Zverev", "Zhuravlev", "Zeltin", "Zlobin", "Zvonkov", "Afonin", "Afanasyev", "Aleksandrov", "Balakirev", "Beresnev",
    "Biryukov", "Chernov", "Chekhov", "Golovin", "Gorbatov", "Kashin", "Khudyakov", "Lisov", "Makarov", "Matvienko",
    "Nekrasov", "Popov", "Romanov", "Shcherbakov", "Stepanov", "Tishchenko", "Tukharev", "Vikhrov", "Zhukov", "Zverev"
}},
{CountryGenerator.Country.Slovenia, new List<string> {
    "Adam", "Aljancic", "Bajda", "Baker", "Barbaric", "Beden", "Bedic", "Bergant", "Bohinc", "Bokal",
    "Bozic", "Bracic", "Bregar", "Brecko", "Brglez", "Bucik", "Buzan", "Cerar", "Cebulj", "Celik",
    "Cernigoj", "Crnic", "Dajnko", "Dolar", "Dolnicar", "Drobnic", "Dukic", "Djordjevic", "Gantar", "Gavran",
    "Goricar", "Gril", "Groselj", "Grum", "Hrovat", "Hribar", "Ivanusic", "Jarc", "Jazbec", "Jeraj",
    "Klemenc", "Knez", "Kobal", "Kocevar", "Kolar", "Kompan", "Koren", "Kovacic", "Kovac", "Krajnc",
    "Kranjc", "Kralj", "Krzisnik", "Lah", "Lukan", "Lukacic", "Lukic", "Lucar", "Lutman", "Majer",
    "Malovrh", "Marinsek", "Medved", "Mesar", "Miklavcic", "Mikulec", "Misic", "Mladenovic", "Murn", "Nedeljkovic",
    "Novak", "Pavlovic", "Peknik", "Pohar", "Potocnik", "Preseren", "Pusnik", "Rajh", "Ravnik", "Rozic",
    "Rutar", "Sef", "Sega", "Sustar", "Sever", "Sircelj", "Skrbin", "Slokan", "Smole", "Snojic",
    "Sodja", "Sole", "Skomina", "Stibilj", "Stefancic", "Svetek", "Svecnik", "Tavcar", "Trcek", "Tomazic",
    "Torelli", "Topic", "Urbanc", "Vajda", "Vidakovic", "Viktor", "Vucko", "Vrsnik", "Zajc", "Zelko",
    "Zgaga", "Zupan", "Zupancic", "Zver", "Zakelj", "Zidan", "Zivkovic", "Zupancic", "Anzic", "Baric",
    "Basic", "Berc", "Bojar", "Borovsak", "Bordjan", "Branko", "Bren", "Breznik", "Bucar", "Cerkvenik",
    "Cec", "Ceh", "Crnek", "Crni", "Dolar", "Drobni", "Erik", "Fajfar", "Fijacko", "Gorenak",
    "Gorib", "Gornik", "Gregor", "Jerman", "Jokan", "Joze", "Kovacic", "Kuhar", "Kolar", "Klemenc",
    "Kraljic", "Knez", "Kos", "Komel", "Kuret", "Lukan", "Lucar", "Maksimovic", "Miklavzic", "Mlakar",
    "Mrezar", "Mihajlovic", "Mole", "Novak", "Rakovec", "Rajh", "Rutar", "Sedenko", "Sikora", "Sabec",
    "Sepec", "Tavcar", "Trnovec", "Toma", "Vuk", "Vovk", "Valic", "Vesel", "Versic", "Vlaovic",
    "Volk", "Vrtovsek", "Zaletel", "Zupancic", "Zorko", "Zupan", "Zupanc", "Zivec", "Zgal", "Bojan",
    "Bracun", "Bostjan", "Blaz", "Barbaric", "Bernik", "Bilandzija", "Bilban", "Bole", "Brezovnik",
    "Blazic", "Gril", "Granda", "Gorosek", "Gros", "Galun", "Grasic", "Habjan", "Huzjan", "Hiter",
    "Humer", "Jance", "Janos", "Kmecic", "Krabek", "Kopcar", "Korosec", "Kosec", "Koselj", "Kranjc",
    "Kralj", "Krizan", "Kos", "Lukovsek", "Lesnik", "Lupsic", "Maticic", "Mikulec", "Miler", "Markovcic",
    "Pehar", "Prelovsek", "Pucnik", "Rajz", "Robic", "Selo", "Slamar", "Skrinjar", "Sodja", "Svojic",
    "Sluga", "Stare", "Svetelj", "Tomsic", "Vajda", "Vukovic", "Vrban", "Vuksan", "Zakosek", "Zemljic"
}}
    };
            #endregion
        }
        #endregion


        #region Static Constructor for Initialization
        static NameGenerator()
        {
            random = RandomNumberGenerator.Create();
            firstNamesByCountry = new Dictionary<CountryGenerator.Country, List<string>>();
            lastNamesByCountry = new Dictionary<CountryGenerator.Country, List<string>>();

            InitializeNameLists();

            // Initialize the HashSets
            usedFirstNames = new HashSet<string>();
            usedLastNames = new HashSet<string>();

            recentlyUsedNames = new Dictionary<CountryGenerator.Country, List<string>>();
            foreach (CountryGenerator.Country country in Enum.GetValues(typeof(CountryGenerator.Country)))
            {
                recentlyUsedNames[country] = new List<string>();
            }
        }
        #endregion

        #region Generate name
        private static int GetRandomIndex(int max)
        {
            byte[] randomNumber = new byte[4];
            random.GetBytes(randomNumber);
            int result = BitConverter.ToInt32(randomNumber, 0);
            return Math.Abs(result % max);
        }

        private static string GetUnusedName(List<string> nameList, HashSet<string> usedNames)
        {
            // Get available names (ones that haven't been used yet)
            var availableNames = nameList.Where(name => !usedNames.Contains(name)).ToList();

            if (availableNames.Count == 0)
            {
                throw new InvalidOperationException("No more unique names available");
            }

            // Select a random name from available names
            string selectedName = availableNames[GetRandomIndex(availableNames.Count)];

            // Add to used names set and remove from original list
            usedNames.Add(selectedName);
            nameList.Remove(selectedName);

            return selectedName;
        }

        public string GenerateRandomName(CountryGenerator.Country country)
        {
            if (!firstNamesByCountry.ContainsKey(country) || !lastNamesByCountry.ContainsKey(country))
            {
                throw new ArgumentException("Country not supported");
            }

            string firstName = GetUnusedName(firstNamesByCountry[country], usedFirstNames);
            string lastName = GetUnusedName(lastNamesByCountry[country], usedLastNames);

            return $"{firstName} {lastName}";
        }
        #endregion
    }
}