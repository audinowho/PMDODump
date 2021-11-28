--[[
  Topics Module
    This module is meant for handling loading all the generic topics that can be used by "Talkers".
    
    Each topic category has its own module, and all those categories are listed inside an XML file "topics.xml", along with details on 
    when to best use a given topic category.
    
--]]
require 'common'

--
Topics = {}
Topics.TopicsFileName = "topics.xml"
Topics.DefaultPriority = 5

--XML Tags names
Topics.RootTag = "Topics"
Topics.TopicTag = "Topic"
Topics.TopicNameTag = 'Name'
Topics.TopicPriorityTag = 'Priority'
Topics.TopicModuleTag = 'ScriptModule'

--[[-----------------------------------------------------------
  LoadTopics
    Load all the topics contained in the topics.xml file into the Topics table returned by this module!
--]]-----------------------------------------------------------
function Topics.LoadTopics()
  local topicstbl = XML:LoadXmlFileToTable(SCRIPT:ServiceDirectoryPath() .. "/chatter/topics/" .. Topics.TopicsFileName)
  assert(topicstbl, "Failed loading the " .. Topics.TopicsFileName .. " file")
  
  --Load topics
--  local cnttopics = 0
--  for k,v in pairs(topicstbl[Topics.TopicTag]) do
--    Topics.LoadSingleTopic(v, cnttopics) 
--    cnttopics = cnttopics + 1
--  end
  
  local topics = topicstbl:GetElementsByTagName(Topics.TopicTag)
  local cnttopics = 0
  while cnttopics < topics.Count do
    Topics.LoadSingleTopicXmlNode(topics:Item(cnttopics), cnttopics) 
    cnttopics = cnttopics + 1
  end
  
  --Report
  PrintInfo("Chatter Engine loaded " .. tostring(cnttopics) .. " topics!")
end

--[[-----------------------------------------------------------
  LoadSingleTopicXmlNode
    Handles a single topic node from the topics xml file!
--]]-----------------------------------------------------------
function Topics.LoadSingleTopicXmlNode(atopic, topicno)
  assert(atopic, "Failed loading topic #" .. tostring(topicno))

  local topicname = XML:GetXmlNodeNamedChild(atopic, Topics.TopicNameTag)
  assert(topicname, "Topic name is null for topic #" .. tostring(topicno))
  topicname = topicname.InnerText
  
  Topics[topicname] = {}
  
  local topicprio = XML:GetXmlNodeNamedChild(atopic, Topics.TopicPriorityTag)
  if topicprio then
    Topics[topicname].priority = tonumber(topicprio.InnerText)
  else
    Topics[topicname].priority = Topics.DefaultPriority
  end
  
  local topicmodpath = XML:GetXmlNodeNamedChild(atopic, Topics.TopicModuleTag)
  assert(topicmodpath, "Topic ScriptModulePath not specified for topic #" .. tostring(topicno))
  assert(topicmodpath.InnerText, "Topic ScriptModulePath is empty or null for topic #" .. tostring(topicno))
  
  Topics[topicname].modulepath = tostring(topicmodpath.InnerText)
    
  --Then load the actual topic module into the table entry
  Topics[topicname]['module'] = require(tostring(Topics[topicname].modulepath))
  
  --
  PrintInfo("Chatter Engine loaded topic " .. topicname)
end


--[[-----------------------------------------------------------
  LoadSingleTopic
    Load a single topic XMLEntry
--]]-----------------------------------------------------------
--function Topics.LoadSingleTopic(atopic, topicno)
--  assert(atopic, "Failed loading topic #" .. tostring(topicno))
  
--  local tname = atopic['Name']
--  assert(tname, "Topic name is null for topic #" .. tostring(topicno))
  
--  Topics[tname] = 
--  {
--    ['priority']      = tonumber(atopic['Priority']),
--    ['modulepath']    = tostring(atopic['ScriptModule']),
--  }
  
--  --Then load the actual topic module into the table entry
--  Topics[tname]['module'] = require(Topics[tname]['modulepath'])
  
--  --
--  PrintInfo("Chatter Engine loaded topic " .. tname)
--end

return Topics