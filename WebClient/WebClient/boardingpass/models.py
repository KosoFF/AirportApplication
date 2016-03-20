from django.db import models

# Create your models here.

class BoardingPass(models.Model): 
    boardingpass_id = models.CharField(max_length=255) 
    passenger_id = models.CharField(max_length=255) 
    checkin_manager_id = models.CharField(max_length=255) 
    flight_id= models.CharField(max_length=255) 
    flight_time = models.CharField(max_length=255)
