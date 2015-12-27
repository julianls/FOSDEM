using FOSDEM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Data.Xml.Dom;
using System.Linq;

namespace FOSDEM
{
    internal class ModelLoader
    {
        private string xml;

        public ModelLoader(string xml)
        {
            this.xml = xml;
        }

        public Conference Conference { get; private set; }

        public void Load()
        {
            this.Conference = new Conference();

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml);

            IXmlNode nodeSchedule = xDoc.SelectSingleNode("schedule");
            IXmlNode nodeConference = nodeSchedule.SelectSingleNode("conference");
            LoadConferenceData(nodeConference);

            XmlNodeList nodesDays = nodeSchedule.SelectNodes("day");
            foreach (IXmlNode item in nodesDays)
            {
                LoadConferenceDayData(item);
            }
        }

        private void LoadConferenceData(IXmlNode nodeConference)
        {
            Conference.Title = nodeConference.SelectSingleNode("title").InnerText;
            Conference.Subtitle = nodeConference.SelectSingleNode("subtitle").InnerText;
            Conference.Venue = nodeConference.SelectSingleNode("venue").InnerText;
            Conference.City = nodeConference.SelectSingleNode("city").InnerText;
            Conference.Start = DateTime.Parse(nodeConference.SelectSingleNode("start").InnerText);
            Conference.End = DateTime.Parse(nodeConference.SelectSingleNode("end").InnerText);
            Conference.NumberOfDays = int.Parse(nodeConference.SelectSingleNode("days").InnerText);
            Conference.DayChange = TimeSpan.Parse(nodeConference.SelectSingleNode("day_change").InnerText);
            Conference.TimeslotDuration = TimeSpan.Parse(nodeConference.SelectSingleNode("timeslot_duration").InnerText);
        }

        private void LoadConferenceDayData(IXmlNode nodeDay)
        {
            Day day = new Day();
            day.Index = int.Parse(nodeDay.Attributes.GetNamedItem("index").InnerText);
            day.Date = DateTime.Parse(nodeDay.Attributes.GetNamedItem("date").InnerText);
            Conference.Days.Add(day);

            XmlNodeList nodesRooms = nodeDay.SelectNodes("room");
            foreach (IXmlNode item in nodesRooms)
            {
                LoadConferenceRoomData(item, day);
            }
        }

        private void LoadConferenceRoomData(IXmlNode nodeRoom, Day day)
        {
            string roomName = nodeRoom.Attributes.GetNamedItem("name").InnerText;
            Room room = Conference.Rooms.FirstOrDefault(item => item.Name == roomName);
            if(room == null)
            {
                room = new Room() { Name = roomName };
                Conference.Rooms.Add(room);
            }

            XmlNodeList nodesEvents = nodeRoom.SelectNodes("event");
            foreach (IXmlNode item in nodesEvents)
            {
                LoadConferenceEventData(item, day, room);
            }
        }

        private void LoadConferenceEventData(IXmlNode nodeEvent, Day day, Room room)
        {
            Event conferenceEvent = new Event();
            conferenceEvent.Id = nodeEvent.Attributes.GetNamedItem("id").InnerText;
            conferenceEvent.Start = day.Date.Add(DateTime.Parse(nodeEvent.SelectSingleNode("start").InnerText).TimeOfDay);
            conferenceEvent.Duration = TimeSpan.Parse(nodeEvent.SelectSingleNode("duration").InnerText);
            conferenceEvent.Room = room;
            conferenceEvent.Day = day;
            conferenceEvent.Slug = nodeEvent.SelectSingleNode("slug").InnerText;
            conferenceEvent.Title = nodeEvent.SelectSingleNode("title").InnerText;
            conferenceEvent.Subtitle = nodeEvent.SelectSingleNode("subtitle").InnerText;
            string trackName = nodeEvent.SelectSingleNode("track").InnerText;
            conferenceEvent.Track = Conference.Tracks.FirstOrDefault(item => item.Name == trackName);
            if (conferenceEvent.Track == null)
            {
                conferenceEvent.Track = new Track() { Name = trackName };
                Conference.Tracks.Add(conferenceEvent.Track);
            }
            string typeName = nodeEvent.SelectSingleNode("type").InnerText;
            conferenceEvent.Type = Conference.EventTypes.FirstOrDefault(item => item.Name == typeName);
            if (conferenceEvent.Type == null)
            {
                conferenceEvent.Type = new EventType() { Name = typeName };
                Conference.EventTypes.Add(conferenceEvent.Type);
            }
            conferenceEvent.Language = nodeEvent.SelectSingleNode("language").InnerText;
            conferenceEvent.Abstract = nodeEvent.SelectSingleNode("abstract").InnerText;
            conferenceEvent.Description = nodeEvent.SelectSingleNode("description").InnerText;

            conferenceEvent.Persons = new List<Person>();
            IXmlNode nodesPersons = nodeEvent.SelectSingleNode("persons");
            XmlNodeList nodesPersonList = nodesPersons.SelectNodes("person");
            foreach (IXmlNode item in nodesPersonList)
            {
                Person person = LoadConferencePersonData(item);
                conferenceEvent.Persons.Add(person);
            }

            conferenceEvent.Links = new List<Link>();
            IXmlNode nodesLinks = nodeEvent.SelectSingleNode("links");
            XmlNodeList nodesLinkList = nodesLinks.SelectNodes("link");
            foreach (IXmlNode item in nodesLinkList)
            {
                Link link = new Link();
                link.Name = item.InnerText;
                link.Url = item.Attributes.GetNamedItem("href").InnerText;
                conferenceEvent.Links.Add(link);
            }

            Conference.Events.Add(conferenceEvent);
        }

        private Person LoadConferencePersonData(IXmlNode nodePerson)
        {
            string personId = nodePerson.Attributes.GetNamedItem("id").InnerText;
            Person person = Conference.Persons.SingleOrDefault(item => item.Id == personId);

            if(person == null)
            {
                person = new Person() { Id = personId };
                person.Name = nodePerson.InnerText;
                Conference.Persons.Add(person);
            }

            return person;
        }
    }
}
