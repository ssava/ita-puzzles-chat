# ITA Hiring Puzzles 

## Simple standalone TCP-based chat server

From [Archive.org](https://web.archive.org/web/20111012115624/http://itasoftware.com/careers/work-at-ita/hiring-puzzles.html)

## Roll Your Own Chat Server

Implement a simple standalone TCP-based chat server, using the following protocol:

The server responds to all commands with either:
```
OK<CRLF>
```

or, when an error occurred, with:

```
ERROR <reason><CRLF> 
```

<CRLF> indicates the bytes '\r\n'.

Upon connecting to the server, the client sends:
```
LOGIN <username><CRLF> 
```


Clients can create new chatrooms or join existing chatrooms (chatrooms begin with the character '#') by doing:

```
JOIN #<chatroom><CRLF>
```

Clients can leave chatrooms by sending:

```
PART #<chatroom><CRLF>
```

Clients can be in multiple chatrooms at once.
Clients can send a message to a chatroom:

```
MSG #<chatroom> <message-text><CRLF> 
```

Clients can send a message to another user:
```
MSG <username> <message-text><CRLF>  
```

When a message is sent to a chatroom the user is in, the server sends to the appropriate client:

```
GOTROOMMSG  <sender> #<chatroom> <message-text><CRLF> 
```

or if the message is sent directly from one user to another:

```
GOTUSERMSG <sender> <message-text><CRLF>  
```

Finally, the client can log off by sending:

```
LOGOUT<CRLF>
```

Here's a transcript of a sample session where a user named "alice" joins a chatroom called #news after connecting. C indicates the line was sent by the client, S indicates it was sent by the server (end of line indicates CRLF was sent):

```
C: LOGIN alice

S: OK

C: JOIN #news

S: OK

C: MSG #news hi everyone

S: GOTROOMMSG bob #news nothing much happened after that

S: OK

S: GOTROOMMSG alice #news hi everyone

S: GOTUSERMSG carol hi alice, where've you been?

C: MSG carol on vacation

S: OK

C: LOGOUT
```


When implementing the server, aim for scalability and robustness. (Many submissions fail due to lack of robustness!) Your submission should include a description of the steps you took towards those two goals. Keep in mind that the client may be buggy, or even malicious. For example, if a client connects to the server and sends an infinite stream of the byte 'X' with no line break, the server should deal with this case gracefully. Please do not use an existing networking framework (e.g., Twisted or asyncore for Python, ACE for C++, etc.) to implement the server. The server should support running on Linux. 
