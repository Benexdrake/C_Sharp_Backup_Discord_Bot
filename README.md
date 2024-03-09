## /backup choose true/false

- Bot sendet Nachrichten an eine Datebank oder an einen Backup Server.
- True erstellt User in DB oder Update User.Backup = true falls vorhanden und dieser auf false steht
- False setzt User.Backup auf false

  - DB:
    - User:
      - ID int
      - Backup bool
      - Admin bool
    
    - Channel:
      - ID int
      - Name string

    - Message:
      - ID int
      - Channel Channel
      - User User
      - Content string
      - Files bool
     
  - Backup Server
    - Bot erstellt Channel und Message
    - Bot sendet jede Nachricht an Webhook, dieser nimmt Name und Avatar vom Message.User an

### Grund wieso nur User ID in DB gespeichert wird ist, dass man sich einen User jederzeit über die Discord API holen kann.
### Idee für eine Website auf der man sich mit seinen Discord Daten anmelden kann um Nachrichten abzurufen, z.B. um Nachrichten zu suchen die wichtige Themen enthalten, sei es im Gaming Tipps und Tricks oder in der Politik wichtige Diagrame, Informationen usw.

### Die Idee eines Backup Bots war, da es sehr leicht ist einen Discord Server sperren zu lassen, es reicht ein Bild was eigentlich Harmlos aussieht, aber die KI von Discord den Server nach Sekunden sperrt.
