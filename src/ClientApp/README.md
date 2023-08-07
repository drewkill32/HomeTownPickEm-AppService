# St. Pete Pickem

## Introduction

St. Pete Pickem is a football pick'em site. The purpose of the site is to allow users to create a league. Each member of the league will then assign themselves a team. Adding a team will add all the games into the league to be picked. Each week, all members of the league will pick the winner (no point-spread) for ALL games in the league. In instances where two teams are playing each other, the game is worth two points. Members can either split their picks or double down on a team. The member with the most points at the end of the season wins.

## Technologies

The backend is built using ASP.NET core and an SQLite database. The backend is hosted in Azure under the free tier.
The front end is built using React and is hosted in Netlify.

## Authentication

Authentication is done using a JWT token and a refresh token. The JWT token has a short lifetime and a refresh token must be used to request a new JWT token. The refresh token has a longer lifetime and is stored in local storage.
