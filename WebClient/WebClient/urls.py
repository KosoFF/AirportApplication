"""
Definition of urls for WebClient.
"""

from django.conf.urls import patterns, include, url
from WebClient.boardingpass import views

# Uncomment the next two lines to enable the admin:
# from django.contrib import admin
# admin.autodiscover()

urlpatterns = patterns('',
    url(r'^$', views.home, name='home'),
    url(r'boardingpass/(?P<pass_id>[-\w]+)$', views.details, name='details'),
    # Examples:
    # url(r'^$', 'WebClient.views.home', name='home'),
    # url(r'^WebClient/', include('WebClient.WebClient.urls')),

    # Uncomment the admin/doc line below to enable admin documentation:
    # url(r'^admin/doc/', include('django.contrib.admindocs.urls')),

    # Uncomment the next line to enable the admin:
    # url(r'^admin/', include(admin.site.urls)),
)
