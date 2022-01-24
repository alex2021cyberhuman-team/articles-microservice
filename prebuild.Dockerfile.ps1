﻿param(
  [String]$Config = 'DEBUG',
  [String]$RuntimeIdentifier = 'linux-x64',
  [String]$tag = 'articles:prebuild',
  [String]$PathToRepository = './'
)
$Publish = "$( $PathToRepository )app/publish/"
dotnet publish "$( $PathToRepository )Conduit.Articles.PresentationLayer" -c $CONFIG -o $Publish -r $RuntimeIdentifier --no-self-contained
docker build -t $tag -f "$( $PathToRepository )prebuild.Dockerfile" $PathToRepository