This website is developed using Apache Forrest version 0.5, located at
  http://forrest.apache.org

To maintain the site, edit documents in the src\documentation\content\xdocs
directory tree. Then, generate the website by making the directory 
containing this readme file the current directory and running 

  forrest
  
The generated website root is located at build\site

Note
----
The src\documentation\skins\ikvmforrest-site folder contains a modified
copy of the forrest-site skin that comes with Forrest. Modifications are
as follows:

* xslt\html\site2xhtml.xsl - modification to show feedback link on the
   bottom of every page