# InstagramService

This is a wrapper over InstagramApiSharp library.
Library provides a simple functionality for downloading media from instagram.
The library is an InstaService class that has dependencies for working with the Instagram API as:

1. InstaAccountSession - to log into your instagram account.
2. InstaMediaProcessor - to work with instagram media.
3. InstaStreamTaker - to take a stream/streams from specified instagram media uri.

Models:

1. InstaMediaStream - represents a model of media.

Collections:

1. InstaMediaStreams - represents a collection of InstaMediaStream objects.

## Installation

```bash
dotnet add package InstagramService
```
