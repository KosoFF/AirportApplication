# Create your views here.
from django.http import HttpResponse 
from django.template.loader import render_to_string 
from django.template import loader 

from WebClient.boardingpass.api import apicall 



def home(request): 
    template = loader.get_template('index.html') 
    context = { 
    'content': apicall.GetBoardingPassArray(), 
    } 
    return HttpResponse(template.render(context, request)) 

def details(request, pass_id): 
    template = loader.get_template('details.html') 
    context = { 
    'content': apicall.GetFullBoardingPass(pass_id) 
    } 
    return HttpResponse(template.render(context, request))
