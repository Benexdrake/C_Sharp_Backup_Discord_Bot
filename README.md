# Commands
## /add_backup_server
#### - Creating a new Server as Backup and insert/update the GuildId into DB
## /add_backup_server id:
#### - Insert/Update a GuildId into DB
## /backup on
#### - Insert Channel ID and more into DB
#### - Creating Webhook for this Channel
#### - Messages in this Channel will Send to DB and send to Webhook with username and avatar from message author
#### - Atachments from Messages will be saved and can be found under ChannelID/MessageUD/
## /backup off
#### - Update Channel in DB to hasBackup = false
## /restore
#### - This function will restore all messages and channels from the Db into a Discord Server, the one in DB
