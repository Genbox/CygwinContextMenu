# Cygwin Context Menu - Shift right click in a folder to open cygwin.

### How to use it
1. Compile the project by pressing CTRL + F5 in Visual Studio.
2. Doubleclick the 'CygwinContextMenu.exe' in the build folder
3. The context menu is now registered. Hold shift and right click in a folder and choose 'Open cygwin window here'
4. Cygwin opens up.

### Notes
Cygwin manual tells us we can use the 'chere' application to register the a context menu in explorer.
See http://code.google.com/p/mintty/wiki/Tips#Starting_mintty_from_a_batch_file

The changes between using 'chere' and this application are very subtle:
* It only works inside folders, and not on folders. This reduces clutter in the menu.
* It uses shift right click, just like the normal 'Open command window here'
* It is named 'Open cygwin window here' instead of 'Open bash shell'