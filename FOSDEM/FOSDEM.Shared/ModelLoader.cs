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
            Event conferemceEvent = new Event();
            conferemceEvent.Id = nodeEvent.Attributes.GetNamedItem("id").InnerText;
            conferemceEvent.Start = DateTime.Parse(nodeEvent.SelectSingleNode("start").InnerText);
            conferemceEvent.Duration = TimeSpan.Parse(nodeEvent.SelectSingleNode("duration").InnerText);
            conferemceEvent.Room = room;
            conferemceEvent.Day = day;
            conferemceEvent.Slug = nodeEvent.SelectSingleNode("slug").InnerText;
            conferemceEvent.Title = nodeEvent.SelectSingleNode("title").InnerText;
            conferemceEvent.Subtitle = nodeEvent.SelectSingleNode("subtitle").InnerText;
            string trackName = nodeEvent.SelectSingleNode("track").InnerText;
            conferemceEvent.Track = Conference.Tracks.FirstOrDefault(item => item.Name == trackName);
            if (conferemceEvent.Track == null)
            {
                conferemceEvent.Track = new Track() { Name = trackName };
                Conference.Tracks.Add(conferemceEvent.Track);
            }
            string typeName = nodeEvent.SelectSingleNode("type").InnerText;
            conferemceEvent.Type = Conference.EventTypes.FirstOrDefault(item => item.Name == typeName);
            if (conferemceEvent.Type == null)
            {
                conferemceEvent.Type = new EventType() { Name = typeName };
                Conference.EventTypes.Add(conferemceEvent.Type);
            }
            conferemceEvent.Language = nodeEvent.SelectSingleNode("language").InnerText;
            conferemceEvent.Abstract = nodeEvent.SelectSingleNode("abstract").InnerText;
            conferemceEvent.Description = nodeEvent.SelectSingleNode("description").InnerText;

            XmlNodeList nodesPersons = nodeEvent.SelectNodes("person");
            foreach (IXmlNode item in nodesPersons)
            {
                Person person = LoadConferencePersonData(item);
                conferemceEvent.Persons.Add(person);
            }

            XmlNodeList nodesLinks = nodeEvent.SelectNodes("link");
            foreach (IXmlNode item in nodesLinks)
            {
                conferemceEvent.Links.Add(item.Attributes.GetNamedItem("href").InnerText + "|"+ item.InnerText);
            }

            Conference.Events.Add(conferemceEvent);
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
