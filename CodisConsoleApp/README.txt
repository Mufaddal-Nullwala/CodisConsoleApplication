***********WELCOME TO CODIS CONSOLE APPLICATION***********
1) Unzip the attached console application .zip file
2) Save the attached XMLFile1.xml and Log.txt file on on your drive - donot chnage the file names and note the drive path
3) Inside the CodisConsoleApp folder, locate and double click the App.Config File.
4) In the App.config file locate the XMLDBPath. Set the drive path for the XMLFile1.xml for the value attribute.
5) In the App.config file locate the LogPath. Set the drive path for the Log.txt for the value attribute.
6) The app.config would look someything like this:
<appSettings>
    <add key="XMLDBPath" value="{YOUR_PATH}\XMLFile1.xml" />
    <add key="LogPath" value="{YOUR_PATH}\Log.txt" />
  </appSettings>
7) Only after successfully saving the Paths, to run the application, go to Bin> Debug>Double click CodisConsoleApp.Application file.
