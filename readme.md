# SkypeRoller

SkypeRoller is an application that hooks into skype and allows you to make dice rolls automatically.

The dice commands should be entered like so:

//1d6 or //2d8+5 or even //10d100-25

You can also make requests of other players to alert them to something they need to action

//Request Perception Check

Would raise an alert to each client running the roller that they need to make a perception check

//Version

Would output the current running version


## Setup

Download the current SkypeRoller build and run the executable contained, or download the project and compile, then run the resulting exe file.

Due to Skype removing the extras manager in later versions, you may get runtime issues if it is not installed, we recommend using skype version 4.2

## Development

This was a simple project done on a train journey, so no tests or build files have been written currently, I will try to add some of these later with a basic pre-built download.

## Future

This meets the criteria I have, so the logic wont change too much unless there is an issue with it, although if anyone is a regex whiz feel free to add support for multiple rolls within one command, like: //(1d20-1d4)+(2d100+10)*2