# clouddownloader
 A simple windows forms application to download files from the internet.
 The principal reasoning behind this application is the explore and apply the fundamentals of concurrency in programming. 
 Parellely running taska and solving issues such as deadlocks between generated virtual worker threads and UI element threads 
 are explored and mitigated through the development of this application.
 
# Functionalities
 - File Downloading: 
   - Files can be downloaded from the web to a directory in the system by copying a link and adding the link to the URL text box and pressing download.
   - Currently supported file formats - **.exe, .jpg, .png, .mp3, .mp4, .ico**
 - Simultaneous Downloads: Multiple files can be downloaded by adding links to the same URL text box, this can be validated with the number of downloads label provided below the text box.
 - The application is capable of using all the threads that are available to the system that the application is running on to initiate as many files as possbile, upon reaching the maximum number of threads, sends out an error message indicating "max downloads reached"
 - File directory can be selected by user before initiating download
 - Progress tracking is done using a label which is located below the download text box. It appears once a download is initiated.
 - Downloads can be cancelled upon initiation.
 - A List view consisting of all downloaded files along with their information is provided in a tabulature format. -> (File Name, Type, Size, Status, Location)
 - Application images are provided at ->
 
# Suggested Future Upgradations
 - Download List can be clickable to locate downloaded file
 - Rather than the downloader using the full potential of the system by default, thread assigning can be added to allow user to choose the number of threads to be used
 - Increase the number of simultaneous downloads
 - UI improvements
 - Progress Bar
 - Support more types of files, ex. larger video files, high quality image files, etc.
 
# How to Run
 - The application can be directly run using the built and generated .exe file
 - To build the program (in the case of an unsuccessful executive throught he .exe file), the file can be opened using visual studio and run for a fresh build. Upon which the program can be run from the IDE or using the newly generated .exe file.

