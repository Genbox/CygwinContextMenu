# Cygwin Context Menu - Shift right click in a folder to open cygwin.

### How to use it
1. Download the application: https://github.com/Genbox/CygwinContextMenu/raw/master/Compiled/CygwinContextMenu.exe
2. Open it.
3. The context menu is now registered.
4. Hold shift and right click in a folder and choose 'Open cygwin window here'

### I want to compile it myself
1. Compile the project by pressing CTRL + F5 in Visual Studio.
2. Doubleclick the 'CygwinContextMenu.exe' in the build folder
3. The context menu is now registered.
4. Hold shift and right click in a folder and choose 'Open cygwin window here'

### Notes
* Åben the CygwinContextMenu.exe application again to unregister it.
* Once registered, you no longer need to keep CygwinContextMenu.exe around.

Cygwin manual tells us we can use the 'chere' application to register the a context menu in explorer.
See http://code.google.com/p/mintty/wiki/Tips#Starting_mintty_from_a_batch_file

The changes between using 'chere' and this application are very subtle:
* It only works inside folders, and not on folders. This reduces clutter in the menu.
* It uses shift right click, just like the normal 'Open command window here'
* It is named 'Open cygwin window here' instead of 'Open bash shell'