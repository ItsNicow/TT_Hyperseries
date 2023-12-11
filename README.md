# Test Technique Hyperseries
## DISCLAIMER

Lors de l'ouverture du projet, si vous rencontrez des bugs, veuillez tout d'abord changer vers la plateforme *Android* dans les *Build Settings* puis relancer le projet Unity. Le build en *.apk* est déjà prêt et disponible dans le dossier *Builds*. Il peut être ouvert et profilé avec *Android Studio*.

Egalement, sont inclus ci-dessous en "titre" des différentes étapes de la réalisation de l'exercice :

ETAPE : TEMPS (TEMPS RECHERCHE & TEMPS RECHERCHE / CREATION ASSETS) puis DESCRIPTION / APPROCHE / RESSENTI

Les temps entre parenthèses font partie du temps total.

## PROGRESS TRACEBACK

### Apprentissage et Documentation : 1h00
J'ai dû ici me familiariser avec les exercices, le type de projet et les nouvelles notions d'UI mobile que je n'avais encore auparavant jamais utilisé. J'ai recherché en profondeur toutes les consignes pour bien comprendre ce qui m'était demandé, puis j'ai ensuite refléchi et fait une retrospection sur comment j'allais procéder.

### Setup UI : 0h30
J'ai commencé le projet par faire un setup tout simple de l'UI, correspondant à ce qui était demandé, mais sans aucun dynamisme. Je l'ai fais le plus scalable et responsive possible, mais ces problèmes se sont rélévés plus contraignants par la suite.

### Swipe : 1h00 (Recherche : 0h30)
Après avoir regardé la vidéo en pièce jointe, j'ai passé du temps à essayer de comprendre le code avant de le recopier, tout en l'adaptant à mon workstyle. J'y ai également ajouté un cap pour ne pas dépasser des deux pages swipables entre elles. Il reste un problème que j'ai essayé en vain de résoudre : si l'on maintien le swipe trop loin, on dépasse la safe zone (on est quand même ramené à la page voulue). J'ai également rendu les boutons *EDITEUR* et *CLIENT* fonctionnels.

### Video Player : 3h (Recherche : 1h & Assets : 0h30)
Ceci étant tout nouveau pour moi, j'ai dû d'abord me familiariser avec l'affichage d'une vidéo. S'en suivit le contrôle du temps (slider, pause, play, restart), puis l'UI (fade in, fade out, temps) sur lequel il reste quelques imperfections si les actions sont performées trop vite. J'ai essayé pendant longtemps d'intégrer le lecteur horizontal automatique avec détection de l'orientation du téléphone, mais ne pouvant pas éxecuter l'*apk* sur mon téléphone, je n'ai pas pu tester cette fonction (je l'ai laissée en commentaire sans y retoucher).

### Series Info Panel Dépliable : 2h30 (Assets : 0h05)
Aaaah les UI responsives, mon pire ennemi. Tout se passait bien jusqu'à ce qu'il faille que je fasse en sorte que les infos de la série soient dépliables et repliables. J'ai eu de gros problèmes pour comprendre pourquoi en 1920x1080 il ne se déplaçait pas comme en 2560x1440 par exemple. Après de longues heures de réflexion, plusieurs siestes, une demi-douzaine de bonnes doses de café, la réponse m'a sauté aux yeux. J'ai immédiatement calculé le ratio hauteur de l'écran / taille du déplacement voulu et toc ! Tout marchait soudainement. J'avais en fait remarqué après un test que mon UI bougeait de 200 pixels constamment, peu importe la taille de l'écran. Il fallait donc que je calcule le déplacement voulu, par rapport au ratio de base de 200/1920.

*Fix : il n'y avait pas besoin du ratio, simplement de la position originale, puis d'y ajouter un déplacement voulu avant d'y revenir...*

### Episodes Panel : 0h30  (Assets : 0h10)
J'ai commencé par tranquillement setup mon UI, dont mon *Scroll Rect*, *Grid Layout*, j'ai ajusté pendant quelques minutes les Content Size Fitters ainsi que les paramètres divers d'ancrage afin que ce panel soit au plus responsive. J'ai reglé quelques masques pour également que le Viewport ne dépasse pas sur le reste lors du dépliage du *Series Info Panel*.

### Episode Class & Episodes Manager : 1h30
Je me suis ensuite atelé à la gestion des épisodes. Ceci fut relativement simple puisque j'avais déjà eu à faire à des situations similaires auparavant. Je suppose que le manque de cafféine a dû me ralentir, haha. :) Il reste encore quelques bugs lorsque par exemple on ouvre le menu pause avant de changer de clip. J'ai essayé de les résoudre, mais cela ne marchait pas, et considérant cela comme quelque chose de relativement mineur (et qui surtout ne devrait pas acaparer tout mon temps), je les ai laissés de côté. J'ai également mis en place la création de nouveaux épisode via la page editeur. Ils ont une thumbnail par défaut ainsi qu'une vidéo par défaut, puisque ces paramètres ne peuvent être manuellement renseignés. S'en suivit l'indication de quel épisode est sélectionné par un petit grisage et hop, le travail était finit.

### Delete Episode Function : 0h30
Par practicité, j'ai deplacé le bouton de sa position originale pour le mettre en bas, et y ai ajouté un petit écran permettant d'éviter les missclicks. J'ai donc commencé par rendre le bouton fonctionnel, puis l'écran tout en m'assurant que le tout soit responsive. J'ai terminé cette fonction par empêcher l'utilisateur de supprimer le dernier épisode disponible, afin d'éviter les bugs.

### Series Info Panel Dépliable Fix : 1h
J'ai réglé ici quelques problèmes avec le menu dépliable, notamment le fait qu'il n'était pas synchronisé avec les épisodes en dessous, et que la longueur du titre n'était pas prise en compte.

### Android Build : 6h (Recherche : 3h & Installations : 1h30)
Cette étape était particulièrement difficile, étant pour moi toute nouvelle. J'ai suivi le tuto en pièce jointe après déjà plusieurs heures d'essai en me documentant par moi-même. Problèmes de version par ci, de SDK par là, enfin bref, je suis passé par tous les maux avant d'y arriver enfin au bout de plusieurs périodes de souffrance à admirer des lignes rouges en abondance. Après l'avoir testé sur mon téléphone et sur celui d'un ami, j'ai remarqué encore quelques problèmes de scalability.

### Fullscreen Button : 1h (Recherche : 0h30 & Assets : 0h05)
J'ai ici mis en place et rendu fonctionnel le bouton permettant de passer le lecteur en mode horizontal et plein écran. Ca a été particulièrement difficile de trouver quelles valeurs il fallait changer, comment, et par rapport à quoi, mais après recherche j'ai de suite trouvé les résultats, malgré avoir longtemps tâtonné par moi-même.

### Fullscreen Detect Rotate : 0h30
J'ai ici ressayé d'implémenter la détection de l'orientation du téléphone, et ça s'est beaucoup mieux passé avec mon nouveau système de fullscreen. J'ai également lock cette feature quand l'utilisateur appuie sur le bouton fullscreen, pour pas empêcher le fullscreen de se mettre correctement. Quelques tests, et c'était fini.

### TEMPS TOTAL : 19h (Recherches : 5h & Assets : 0h50 & Installations : 1h30)

## ARCHITECTURE  

L'architecture de mon canvas se décompose en pages puis en zones. J'ai un conteneur pour mes pages (donc l'éditeur et le player), puis dans chaque page j'ai un objet pour mes zones. Les zones peuvent elles-mêmes contenir d'autres zones, comme par exemple la PlayerArea. Afin de gérer proprement le switch horizontal, j'ai séparé les éléments de la VideoPlayerPage qui n'étaient pas de la PlayerArea dans une autre zone. Certains éléments n'appartiennent pas à une zone, comme le ClientButton ou le WarningMessage, et ce parce que je ne trouvais pas cela nécessaire, et moins time-efficient.

L'architecture de mon code se décompose principalement en Managers qui permettent de gérer différents aspects des fonctionnalités, et je dispose d'une classe Episode pour ma prefab d'épisode. Le PageSwiper est en charge du swipe entre les pages, y compris avec les boutons CLIENT et EDITEUR. Mon EditorManager se charge simplement de placer correctement la page éditeur au début (bien qu'il aurait fallu dedans que je handle les inputs fields et la création d'épisodes). Ma classe Episode contient toutes les infos relatives à un épisode et des méthodes pour pouvoir notamment le sélectionner, le désélectionner ou le supprimer. Mon VideoPlayerManager gère beaucoup de choses, comme notamment (et évidemment) le VideoPlayer, le fullscreen, le WarningMessage ainsi que l'adaptation du player en fonction de l'épisode sélectionné. Mon EpisodesListManager gère la liste des épisodes et également implémente la méthode de création d'un épisode. Finalement, mon SeriesInfoManager se charge du dépliage et repliage du panel dépliable des informations de la série / de l'épisode.

## EXPERIENCE FEEDBACK

Ayant pu travailler sur quelque chose qui me plaît, c'est-à-dire de l'UI, ce projet était très fun à réaliser malgré les longues heures de recherche et réflexion. J'ai pu me familiariser avec 2-3 aspects de l'UI ou encore des applis mobiles dont je n'avais pas connaissance, et j'ai trouvé cela très instruisant. En somme, j'ai eu une plutôt bonne expérience avec ce test technique. Il reste encore quelques bugs, comme notamment avec l'overlay de pause qui parfois fade out au mauvais moment, et encore quelques bugs très niches (par exemple, si l'on commence à swipe avant de tourner le téléphone, l'UI bug complètement). Je pense que si dans une vie je devais le refaire, je ne serais pas contre et serais beaucoup plus efficient grâce à tout ce que ce test m'a apporté, tant en connaissances qu'en familiarité avec l'UI, ce qui me permettra par la suite d'être plus efficient dans ce domaine. Je pense tout de même avoir encore beaucoup à apprendre, notamment en termes d'organisation du canvas, du code et de ses différents compartiments / fonctionnalités.
