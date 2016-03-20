from WebClient.boardingpass.api import helpers 


import requests 


def GetBoardingPassArray(): 
    lst=[] 
    temp=requests.get(helpers._url('boardingpass')).json()['GetBoardingPassResult']; 
    for passenger in temp: 
        lst.append(helpers.BoardingPassConvert(passenger)); 
    return lst; 


class FullBoardingPass: 
    def __init__(self, **entries): 
        self.__dict__.update(entries) 
def GetFullBoardingPass(str): 
    temp=requests.get(helpers._url('boardingpass/{}'.format(str))).json()['GetBoardingPassByIdResult'] 
    p = FullBoardingPass(**temp) 
    return p;