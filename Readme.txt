Projektname: 	Entwicklung einer VR-Umgebung für explorative Nutzung einer blockorienterten Programmieroberfläche zur Drohnen-Steuerung
Kurzname:	ThesisCH23
Teammitglieder:	Calvin Hofmann

Link zu GitHub-Repository:
https://github.com/cahofman13/ThesisCH23

-----------------

Unter dem Link befinden sich das GitHub-Repository des Projekts.
Die finale Version zum Zeitpunkt der Abgabe ist 'release-1.0.0'.
Ein fertiger APK-Build für die Quest befindet sich im Root des Verzeichnisses.

Das Projekt ist momentan für den Build einer APK für die Oculus Quest eingestellt. 
Die Player und OpenXR Settings müssten bei anderen Brillen umgestellt werden.

Das Unity-Projekt beinhaltet mehrere Szenen als Zwischenschritte/Backup, die finale VR-Szenen ist 'VRScene'.

-----------------

Verwendete Assets und zusätzliche Packages:

 - com.unity.textmeshpro
 - com.unity.ugui
 - com.unity.xr.openxr
 - com.unity.xr.interaction.toolkit

 - Wand-Textur "Patterned Brick Wall" von Dario Barresi & Dimitrios Savva:	
	https://polyhaven.com/a/patterned_brick_wall

 - Sounds "Beep 6" und "success_02" by Pixabay:
	https://pixabay.com/sound-effects/beep-6-96243/
	https://pixabay.com/sound-effects/success-02-68338/

-----------------

Insgesamt lief der Entwicklungsprozess gut. 
Es gab keine größeren Schwierigkeiten oder explizite "Roadblocks", daher wurde auch nicht auf fremde Lösungen verwiesen.

Das Programmsystem (Interpreter & Anweisungen) wurde von Grund auf erdacht und ist nicht perfekt, ist jedoch robust und nicht übermäßig kompliziert.
Die Entwicklung der Umgebung sowie der VR-Steuerung lief ebenfalls recht glatt.

Das Hauptproblem war der zeitliche Aufwand, der in die Programmeingabe (Compiler & Eingabeobjekte) fließen musste. 
Das Entwerfen des Aufbaus der Eingabeobjekte (v.a. der Nodes, aber auch der UI-Blöcke) kostete Einiges an Zeit.
Dazu kamen noch die vielen Varianten der Eingabeobjekte, die individuell trotzdem angepasst werden mussten (for, if, write, etc.).
Aufgrund dieses Zeitbedarfs wurde entsprechend die Funktionalität der Drohne limitiert.

Der Aspekt, der allein am meisten Probleme bereitete, war das Speichersystem.
Hier kam es bei dem Einstellen der Variablen in den Nodes sehr häufig - durch verschiedene Auslöser - 
zu Asynchronität zwischen der angezeigten und benutzten Variable.

Eine zentrale Erkenntnis des Projekts fand ich, den "Trade-Off" zwischen guter Bedienbarkeit (der Steuerung) und Mächtigkeit des Programmsystems.
Dadurch, dass Elemente in VR eine gewisse Größe benötigen, ist es schwer viel Inhalt auf wenig Fläche zu bekommen, ohne die Steuerungsmethoden auszuweiten.
Entsprechend muss sich bei visueller Programmierung in VR entschieden werden, 
ob mehr Wert auf eine simple Steuerung gelegt wird oder es wichtiger ist, dass die Nodes/Blöcke komplexe Informationen enthalten können.
Ersteres ist dabei besser für Anfänger, verhindert aber präzise Werteingaben für Nodes/Blöcke.
Dies fiel mir erst gegen Ende durch die Tests mit Verwandten & Freunden auf,  
da ich zuvor bei dem Design der Steuerung den Fokus auf die Präzision gelegt habe und deswegen eine direkte "Greif"-Steuerung in den Fokus gelegt habe.
