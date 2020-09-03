import os
import time
from termcolor import colored

os.system('title Prime')
def resumeCalculating(numFile, file):
    readNum = open(numFile, 'r')
    for element in readNum:
        if 'lastNum' in element:
            lastNum = element.strip().split(' = ')[1]
    num = int(lastNum) + 1
    write = open(file, 'a')
    while True:
        if num > 1:
           # check for factors
           for i in range(2,num):
               if (num % i) == 0:
                   break
           else:
               print(num, file=write)
               print(num)

        if (num % 100) == 0:
            save = open(numFile, 'w')
            print('lastNum =', num, file=save)
            save.close()
        if (num % 1000) == 0:
            print('\n|=========================================|')
            print(colored(' SAFE TIME: 5 seconds', "green"))
            time.sleep(1)
            print(colored(' SAFE TIME: 4 seconds', "green"))
            time.sleep(1)
            print(colored(' SAFE TIME: 3 seconds (LAST CHANCE)', "green"))
            time.sleep(1)
            print(colored(' SAFE TIME: 2 seconds (WARNING)', "yellow"))
            time.sleep(1)
            print(colored(' SAFE TIME: 1 second  (WARNING)', "red"))
            time.sleep(1)
            print(colored(' SAFE TIME: NONE      (WAIT FOR NEXT TIME)', "red"))
            print('|=========================================|')
        num += 1
resumeCalculating('PrimeLastNum', 'PrimeNumbers.txt')