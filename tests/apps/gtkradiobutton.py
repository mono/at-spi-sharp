#!/usr/bin/env python

# example gtkradiobutton.py

import pygtk
pygtk.require('2.0')
import gtk

class RadioButton:
    def delete_event(self, widget, event, data=None):
        gtk.main_quit()
        return False

    def __init__(self):
        self.window = gtk.Window(gtk.WINDOW_TOPLEVEL)
        self.window.set_title("Radio Button")
        self.window.connect("delete_event", self.delete_event)
        self.window.set_border_width(12)

        vbox = gtk.VBox()
        vbox.set_spacing(6)

        radio1 = gtk.RadioButton(None, "Apple")
        radio2 = gtk.RadioButton(radio1, "Banana")
        radio3 = gtk.RadioButton(radio1, "Cherry")

        # four more radio buttons that are not associated with the previous 3
        radio4 = gtk.RadioButton(None, "Fee")
        radio5 = gtk.RadioButton(radio4, "Fie")
        radio6 = gtk.RadioButton(radio4, "Foe")
        radio7 = gtk.RadioButton(radio4, "Fum")

        vbox.add(radio1)
        vbox.add(radio2)
        vbox.add(radio3)

        vbox.add(radio4)
        vbox.add(radio5)
        vbox.add(radio6)
        vbox.add(radio7)

        self.window.add(vbox)

        vbox.show_all()
        self.window.show()

def main():
    gtk.main()
    return 0

if __name__ == "__main__":
    RadioButton()
    main()
