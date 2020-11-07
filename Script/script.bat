curl 127.0.0.1:6145/
curl 127.0.0.1:6145/messages
curl 127.0.0.1:6145/messages/1
curl 127.0.0.1:6145/messages/1/3
curl 127.0.0.1:6145/messages/1f3

curl -X POST -d "Hallo Wie gehts" 127.0.0.1:6145/messages
curl -X POST -d "Hallo Wie gehts2" 127.0.0.1:6145/messages/1
curl -X POST -d "Hallo Wie gehts3" 127.0.0.1:6145/messages/b312
curl -X POST -d "" 127.0.0.1:6145/messages/b312

curl 127.0.0.1:6145/messages
curl 127.0.0.1:6145/messages/1
curl 127.0.0.1:6145/messages/3

curl -X PUT -d "Bearbeitet" 127.0.0.1:6145/messages/2
curl -X PUT -d "Bearbeitet" 127.0.0.1:6145/messages/2b
curl -X PUT -d "" 127.0.0.1:6145/messages/1


curl -X DELETE 127.0.0.1:6145/messages/1
curl -X DELETE 127.0.0.1:6145/messages/1b
curl -X DELETE 127.0.0.1:6145/messages/1
curl -X DELETE -d "test"  127.0.0.1:6145/messages/1
curl -X DELETE -d "test"  127.0.0.1:6145/messages/3

curl 127.0.0.1:6145/messages
curl 127.0.0.1:6145/messages/2
