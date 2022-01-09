@echo off

REM --------------------------------------------------
REM Monster Trading Cards Game
REM --------------------------------------------------
title Monster Trading Cards Game
echo CURL Testing for Monster Trading Cards Game
echo.

REM --------------------------------------------------
echo 1) Create Users (Registration)
REM Create User
curl -X POST http://localhost:8080/Player --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\",\"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:8080/Player --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\",\"Password\":\"markus\"}"
echo.
curl -X POST http://localhost:8080/Player --header "Content-Type: application/json" -d "{\"Username\":\"admin\",\"Password\":\"istrator\"}"
echo.

echo should fail:
curl -X POST http://localhost:8080/Player --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:8080/Player --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
echo.
echo.

REM --------------------------------------------------
echo 2) Login Users
curl -X POST http://localhost:8080/Login --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:8080/Login --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo.
curl -X POST http://localhost:8080/Login --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo.

echo should fail:
curl -X POST http://localhost:8080/Login --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
echo.
echo.

REM --------------------------------------------------
echo 3) create packages (done by "admin")
curl -X PUT http://localhost:8080/Pack --header "Content-Type: application/json" --header "Authorization: admin-Token" -d "{\"cards\": \"6,7,22,8,9\"}"
echo.
echo.

REM --------------------------------------------------
echo 4) acquire packages kienboec
curl -X POST http://localhost:8080/Pack --header "Content-Type: application/json" --header "Authorization: kienboec-Token" -d "{\"id\": \"1\"}"
echo.
curl -X POST http://localhost:8080/Pack --header "Content-Type: application/json" --header "Authorization: kienboec-Token" -d "{\"id\": \"2\"}"
echo.
curl -X POST http://localhost:8080/Pack --header "Content-Type: application/json" --header "Authorization: kienboec-Token" -d "{\"id\": \"3\"}"
echo.
curl -X POST http://localhost:8080/Pack --header "Content-Type: application/json" --header "Authorization: kienboec-Token" -d "{\"id\": \"4\"}"
echo.
echo should fail (no money):
curl -X POST http://localhost:8080/Pack --header "Content-Type: application/json" --header "Authorization: kienboec-Token" -d "{\"id\": \"5\"}"
echo.
echo.

REM --------------------------------------------------
echo 5) acquire packages altenhof
curl -X POST http://localhost:8080/Pack --header "Content-Type: application/json" --header "Authorization: altenhof-Token" -d "{\"id\": \"2\"}"
echo.
curl -X POST http://localhost:8080/Pack --header "Content-Type: application/json" --header "Authorization: altenhof-Token" -d "{\"id\": \"3\"}"
echo.
echo should fail (no package):
curl -X POST http://localhost:8080/Pack --header "Content-Type: application/json" --header "Authorization: altenhof-Token" -d "{\"id\": \"-1\"}"
echo.
echo.

REM --------------------------------------------------
echo 6) add new packages //toDo
curl -X PUT http://localhost:8080/Pack --header "Content-Type: application/json" --header "Authorization: admin-Token" -d "{\"cards\": \"22,25,23,11,19\"}"
echo.

REM --------------------------------------------------
echo 7) acquire newly created packages altenhof //will also refund duplicates
curl -X POST http://localhost:8080/Pack --header "Content-Type: application/json" --header "Authorization: altenhof-Token" -d "{\"id\": \"12\"}"
echo.
curl -X POST http://localhost:8080/Pack --header "Content-Type: application/json" --header "Authorization: altenhof-Token" -d "{\"id\": \"13\"}"
echo.
echo.

REM --------------------------------------------------
echo 8) show all acquired cards kienboec
curl -X GET http://localhost:8080/Collection --header "Authorization: kienboec-Token"
echo.
echo should fail (no token)
curl -X GET http://localhost:8080/Collection
echo.
echo.

REM --------------------------------------------------
echo 9) show all acquired cards altenhof
curl -X GET http://localhost:8080/Collection --header "Authorization: altenhof-Token"
echo.
echo.

REM --------------------------------------------------
echo 10) show unconfigured deck
curl -X GET http://localhost:8080/Deck --header "Authorization: kienboec-Token"
echo.
curl -X GET http://localhost:8080/Deck --header "Authorization: altenhof-Token"
echo.
echo.

REM --------------------------------------------------
echo 11) configure deck
curl -X PUT http://localhost:8080/Deck --header "Content-Type: application/json" --header "Authorization: kienboec-Token" -d "{\"cards\": \"0,1,2,3\"}"
echo.

echo.
curl -X PUT http://localhost:8080/Deck --header "Content-Type: application/json" --header "Authorization: altenhof-Token" -d "{\"cards\": \"6,7,8,9\"}"
echo.
echo.
echo.
echo should fail and show original from before:
curl -X PUT http://localhost:8080/Deck --header "Content-Type: application/json" --header "Authorization: altenhof-Token" -d "{\"cards\": \"1,1,1,4\"}"
echo.
curl -X GET http://localhost:8080/Deck --header "Authorization: altenhof-Token"
echo.
echo.
echo should fail ... only 3 cards set
curl -X PUT http://localhost:8080/Deck --header "Content-Type: application/json" --header "Authorization: altenhof-Token" -d "{\"cards\": \"6,7,8\"}"
echo.


REM --------------------------------------------------
echo 12) show configured deck
curl -X GET http://localhost:8080/Deck --header "Authorization: kienboec-Token"
echo.
curl -X GET http://localhost:8080/Deck --header "Authorization: altenhof-Token"
echo.
echo.

REM --------------------------------------------------
echo 14) View user Data
echo.
echo.
echo should not fail:
curl -X GET http://localhost:8080/Player?name=kienboec
echo.
curl -X GET http://localhost:8080/Player?name=altenhof
echo.

REM --------------------------------------------------
echo 15) stats
curl -X GET http://localhost:8080/Ranking
echo.
echo.

REM --------------------------------------------------
echo 16) scoreboard
curl -X GET http://localhost:8080/Ranking
echo.
echo.

REM --------------------------------------------------
echo 17) battle
start /b "kienboec battle" curl -X POST http://localhost:8080/EnterMM --header "Authorization: kienboec-Token"
start /b "altenhof battle" curl -X POST http://localhost:8080/EnterMM --header "Authorization: altenhof-Token"
ping localhost -n 10 >NUL 2>NUL
echo.
REM --------------------------------------------------
echo.
echo 18) Stats
echo kienboec
curl -X GET http://localhost:8080/Player?name=kienboec
echo.
echo altenhof
curl -X GET http://localhost:8080/Player?name=altenhof
echo.
echo.

REM --------------------------------------------------
echo 19) scoreboard
curl -X GET http://localhost:8080/Ranking
echo.
echo.

REM --------------------------------------------------
echo 20) trade
echo check trading deals
curl -X GET http://localhost:8080/Trades
echo.
echo Try Create Trade (should Fail)
curl -X POST http://localhost:8080/Trade --header "Content-Type: application/json" --header "Authorization: kienboec-Token" -d "{\"Params\": \"2,-1,2\"}"
echo.
echo Update Deck
curl -X PUT http://localhost:8080/Deck --header "Content-Type: application/json" --header "Authorization: kienboec-Token" -d "{\"cards\": \"0,1,4,3\"}"
echo.
echo try again to create trading deal
curl -X POST http://localhost:8080/Trade --header "Content-Type: application/json" --header "Authorization: kienboec-Token" -d "{\"Params\": \"2,-1,2\"}"
echo.
echo check trading deals
curl -X GET http://localhost:8080/Trades
echo.
echo delete trading deals
curl -X DELETE http://localhost:8080/Trade --header "Content-Type: application/json" --header "Authorization: kienboec-Token" -d "{\"id\": \"1\"}"
echo.
echo.

REM --------------------------------------------------
echo 21) check trading deals
curl -X GET http://localhost:8080/Trades
echo.
echo 22) create trading deal for money
curl -X POST http://localhost:8080/Trade --header "Content-Type: application/json" --header "Authorization: kienboec-Token" -d "{\"Params\": \"2,-1,2\"}"
echo check trading deals
curl -X GET http://localhost:8080/Trades
echo.
echo try to trade with yourself
curl -X POST http://localhost:8080/Trade --header "Content-Type: application/json" --header "Authorization: kienboec-Token" -d "{\"id\": \"2\"}"
echo.
echo accept the trade
curl -X POST http://localhost:8080/Trade --header "Content-Type: application/json" --header "Authorization: altenhof-Token" -d "{\"id\": \"2\"}"
echo check trading deals
echo.
curl -X GET http://localhost:8080/Trades
echo.
echo 23) create trading deal for card
curl -X POST http://localhost:8080/Trade --header "Content-Type: application/json" --header "Authorization: kienboec-Token" -d "{\"Params\": \"16,2,0\"}"
echo.
echo check trading deals
echo.
curl -X GET http://localhost:8080/Trades
echo.
echo accept the trade
curl -X POST http://localhost:8080/Trade --header "Content-Type: application/json" --header "Authorization: altenhof-Token" -d "{\"id\": \"3\"}"
echo.
REM --------------------------------------------------
echo end...

REM this is approx a sleep
ping localhost -n 100 >NUL 2>NUL
@echo on
