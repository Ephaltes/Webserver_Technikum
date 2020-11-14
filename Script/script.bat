::Fail Tests
curl 127.0.0.1:6145/
curl 127.0.0.1:6145/messages
curl 127.0.0.1:6145/messages/1
curl 127.0.0.1:6145/messages/1/3
curl 127.0.0.1:6145/messages/1f3

::Insert Successfull
curl -X POST -d "Hallo Wie gehts" 127.0.0.1:6145/messages
curl -X POST -d "Hallo Wie gehts2" 127.0.0.1:6145/messages/1
curl -X POST -d "Hallo Wie gehts3" 127.0.0.1:6145/messages/b312

::Fail because Empty Body
curl -X POST -d "" 127.0.0.1:6145/messages/b312

::Show messages
curl 127.0.0.1:6145/messages
curl 127.0.0.1:6145/messages/1
curl 127.0.0.1:6145/messages/3

curl -X PUT -d "Bearbeitet" 127.0.0.1:6145/messages/2
::Fail Update
curl -X PUT -d "Bearbeitet" 127.0.0.1:6145/messages/2b
curl -X PUT -d "" 127.0.0.1:6145/messages/1


curl -X DELETE 127.0.0.1:6145/messages/1
::Fail because Wrong id/already deleted
curl -X DELETE 127.0.0.1:6145/messages/1b
curl -X DELETE 127.0.0.1:6145/messages/1
curl -X DELETE -d "test"  127.0.0.1:6145/messages/1
::Successfull
curl -X DELETE -d "test"  127.0.0.1:6145/messages/3

::Show messages
curl 127.0.0.1:6145/messages
curl 127.0.0.1:6145/messages/2
