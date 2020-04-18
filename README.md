# BcFamilyAlbum - the backend service

# Changelog
```
0.3.1 2020-04-15 36dd0dcb FIX workaround some problems with storing not null default-valued timestamps in SQLite
                          remove empty directories from album data returning to the frontend
0.3.0 2020-04-13 88d1cd3b FIX better CORS support, introduce DB migrations
                          store info about files deleted from the album to ignore them in future
0.2.0 2020-04-12 e60cef54 Add prototype of a persistence layer through EF Core and SQLite
                          Reorganize source code repository structure
0.1.1 2020-04-11 03e1a378 Returning pictures and streaming videos to the frontend
0.1.0 2020-04-09 07e04b97 First version that actually return album data, changes in data format 
0.0.2 2020-04-07 7f253fc9 Returning mocked data via dedicated provider, add CORS handling
0.0.1 2020-04-06 c0fb206a First version, returning mocked data
```
