# Student Record Management System #

## Sections: ##
* [User Page](#user-page)
* [Administrator Page](#administrator-page)
* [Faculty Page](#faculty-page)
* [Student Page](#student-page)
* [Sign In](#sign-in)
* [Update](#update)
* [Delete](#delete)
* [Search](#search)
* [Sign Out](#sign-out)

## Introduction ##
This is a CRUD system using ASP.NET with jQuery and AJAX for requesting data from endpoints. Object-Oriented Programming (OOP) was applied to manipulate the database with Object Relational Mapping (ORM). JSON Web Token (JWT) was used to generate an access token for the authorization header.

## Features ##
### User Page ###
There are three types of users: student, faculty, and administrator.
### Administrator Page ###
The administrator is responsible for adding academic member accounts and basic information. The added accounts are used to enable academic members to sign in to the system.
### Faculty Page ###
The faculty is responsible for adding student records.
### Student Page ###
The student is only allowed to see their own records.
### Sign In ###
During sign-in, the server generates an access token, and the client-side accepts and stores this token temporarily.
### Admin ###
### Faculty ###
### Student ###
### Update ###
During an update, the server checks for data conflicts before updating the database.
### Update Member ###
### Update Record ###
### Delete ###
The system warns the user before performing the deletion.
### Search ###
The system checks for any possible keywords in the database and returns the filtered data.
### Sign Out ###
The system clears the access token that was stored on the client-side.

## ScreenShots ##
![user_page](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/user%20page.jpg)
![admin_page](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/administrator%20page.jpg)
![faculty_page](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/faculty%20page.jpg)
![student_page](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/student%20page.jpg)
![admin_signin](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/admin%20-%20signin.jpg)
![faculty_signin](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/faculty-signin.jpg)
![student_signin](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/student-signin.jpg)
![update_member](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/update%20member.jpg)
![update_record](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/update%20record.jpg)
![delete_record](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/delete.jpg)
![search](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/administrator%20search.jpg)
![sign_out](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/signout.jpg)
