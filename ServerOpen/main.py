from body import BodyThread
import time
import global_vars
from sys import exit

#global_vars.CAMERA=input("Camera number: ")
global_vars.CAMERA=0
thread = BodyThread()
thread.start()

i = input()
print("Exiting…")
global_vars.KILL_THREADS = True
time.sleep(0.5)
exit()