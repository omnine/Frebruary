# FREB Viewer

## Introduction

A simple Failed Request Tracing Viewer and Filtering Tool.

We used to use Internet Explorer to view IIS FREB logs, but it was retired on June 15, 2022.

Unfortunately the successor, MS Edge, doesn't work in this case due to its security enhancement.

This project was forked from, I added WebView2 support.

For backward compatibility, The WebBrowser (Embedded IE Control) is kept.

## The prerequisites

.NET Framework 4.5 is needed.

## How to use it?

- Download and run the app;
- Browser to the FREB log folder;
- Scan the logs under that folder;
- Choose one log in the list, then Preview it.

## References

[Script Blocks Using msxsl:script](https://learn.microsoft.com/en-us/dotnet/standard/data/xml/script-blocks-using-msxsl-script)
[XSLT Stylesheet Scripting Using <msxsl:script>](https://github.com/foxbot/dotnet-docs/blob/master/docs/standard/data/xml/xslt-stylesheet-scripting-using-msxsl-script.md)