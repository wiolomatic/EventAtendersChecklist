**How to solve the Selenium Exception when creating IE driver**

You need to set same protected mode in all zones.

1. { IE -> Tools -> Internet Options } or { Control Panel -> Internet Options } -> Security

2. For all zones (Internet, Local intranet, Trusted sites, Restricted sites) set "Enable Protected Mode" (on/off doesn't matter, just set everything the same)
