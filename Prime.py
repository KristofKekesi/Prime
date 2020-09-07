import os
import time
from termcolor import colored

os.system('title Prime')
def resumeCalculating(numFile, file):
	readNum = open(numFile, 'r')
	for element in readNum:
		if 'lastNum' in element:
			lastNum = element.strip().split(' = ')[1]
		elif 'lastEllapsedTime' in element:
			lastEllapsedTime = element.strip().split(' = ')[1]
		elif 'lastFoundPrime' in element:
			lastFoundPrime = element.strip().split(' = ')[1]
		elif 'lastTimePerPrime' in element:
			lastTimePerPrime = element.strip().split(' = ')[1]
	num = int(lastNum) + 1

	foundPrime = 0
	startTime = time.time()
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
			   foundPrime += 1

		if (num % 1000) == 0:
			endTime = time.time()
			ellapsedTime = endTime - startTime
			timePerPrime = ellapsedTime / foundPrime
			
			save = open(numFile, 'w')
			print('lastNum =', num, file=save)
			print('lastEllapsedTime =', ellapsedTime, file=save)
			print('lastFoundPrime =', foundPrime, file=save)
			print('lastTimePerPrime =', str(timePerPrime), file=save)
			save.close()
			
			write.close()
			print('\n|==============================================|')
			if float(lastEllapsedTime) > ellapsedTime:
				print(' > Ellapsed Time:       ' + colored(str(ellapsedTime), "green"))
			elif float(lastEllapsedTime) < ellapsedTime:
				print(' > Ellapsed Time:       ' + colored(str(ellapsedTime), "red"))
			else:
				print(' > Ellapsed Time:       ' + colored(str(ellapsedTime), "yellow"))
			
			if float(lastFoundPrime) < foundPrime:
				print(' > Found Prime Numbers: ' + colored(str(foundPrime), "green"))
			elif float(lastFoundPrime) > foundPrime:
				print(' > Found Prime Numbers: ' + colored(str(foundPrime), "red"))
			else:
				print(' > Found Prime Numbers: ' + colored(str(foundPrime), "yellow"))
			
			if float(lastTimePerPrime) > timePerPrime:
				print(' > Time per Prime:      ' + colored(str(timePerPrime), "green"))
			elif float(lastTimePerPrime) < timePerPrime:
				print(' > Time per Prime:      ' + colored(str(timePerPrime), "red"))
			else:
				print(' > Time per Prime:      ' + colored(str(timePerPrime), "yellow"))
				
			print('|==============================================|')
			print(' SAFE TIME: ' + colored('5 seconds', "green"))
			time.sleep(1)
			print(' SAFE TIME: ' + colored('4 seconds', "green"))
			time.sleep(1)
			print(' SAFE TIME: ' + colored('3 seconds (LAST CHANCE)', "green"))
			time.sleep(1)
			print(' SAFE TIME: ' + colored('2 seconds (WARNING)', "yellow"))
			time.sleep(1)
			print(' SAFE TIME: ' + colored('1 second  (WARNING)', "red"))
			time.sleep(1)
			print(' SAFE TIME: ' + colored('NONE      (WAIT FOR NEXT TIME)', "red"))
			print('|==============================================|\n')
			lastEllapsedTime = ellapsedTime
			lastFoundPrime = foundPrime
			lastTimePerPrime = timePerPrime
			
			foundPrime = 0
			startTime = time.time()
			write = open(file, 'a')
		num += 1
resumeCalculating('PrimeLastNum', 'PrimeNumbers.txt')