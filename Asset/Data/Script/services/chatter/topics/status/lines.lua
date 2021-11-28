--[[
    Contains definitions for the possible lines that a talking NPC may use
--]]
local BaseLine = require 'services.chatter.baseline'
local Statuses = {}

--------------------------------------------------------
-- Health Related
--------------------------------------------------------

--[[-----------------------------------------------------
    Healthy dialogue
      Says something about how they are in good shape.
--]]-----------------------------------------------------
Statuses.Healthy = Class("Healthy", BaseLine)

function Statuses.Healthy:initialize()
  self.AlreadySaid = false
end

--[[
    Predicate
      function determining whether this line's talk method should be
      executed at all.
--]]
function Statuses.Healthy:Predicate(talker)
  return (not self.AlreadySaid) and (talker.HP >= (talker.MaxHP / 2))
end

--[[
    Talk
      Executes this line choregraphy!
      Ran as a coroutine, so usual choregraphy script functions work here!
--]]
function Statuses.Healthy:Talk(talker, activator)
    UI:SetSpeaker(talker)
    --#TODO: pick a random line fitting the kind of personality, specie, and etc we have
    UI:WaitShowDialogue(STRINGS:Format("I'm doing good [PLAYERNAME]!"))
    self.AlreadySaid = true
end
  
--[[ 
    Relevance
      Relevance of the line. Its basically a modifier added to the base priority of the
      topic. Higher values are less relevant. Less relevant topics are less likely to 
      be said than more relevant ones.
--]]
function Statuses.Healthy:Relevance(talker) 
  return 10 
end

--[[-----------------------------------------------------
    Hurt dialogue
      Says something about how they took some damages
--]]-----------------------------------------------------
Statuses.Hurt = Class("Hurt", BaseLine)

function Statuses.Hurt:initialize()
  self.AlreadySaid = false
end

function Statuses.Hurt:Predicate(talker)
  --Reset Already said if we went back over 50% in the meantime
  if talker.HP >= (talker.MaxHP / 2) then
    self.AlreadySaid = false
  end
  return (not self.AlreadySaid) and (talker.HP < (talker.MaxHP / 2)) and (talker.HP > (talker.MaxHP * 0.2))
end

function Statuses.Hurt:Talk(talker, activator)
  UI:SetSpeaker(talker)
  --#TODO: pick a random line fitting the kind of personality, specie, and etc we have
  UI:WaitShowDialogue(STRINGS:Format("I really took a beating.. Hopefully things will calm down so I can recover!"))
  self.AlreadySaid = true
end

function Statuses.Hurt:Relevance(talker) 
  return 0
end


--[[-----------------------------------------------------
    BadlyHurt dialogue
      Says something about how they took serious damages
--]]-----------------------------------------------------
Statuses.BadlyHurt = Class("BadlyHurt", BaseLine)

function Statuses.BadlyHurt:initialize()
  self.AlreadySaid = false
end

function Statuses.BadlyHurt:Predicate(talker)
  --Reset Already said if we went back over 20% in the meantime
  if talker.HP > 0.2 then
    self.AlreadySaid = false
  end
  
  return (not self.AlreadySaid) and (talker.HP <= (talker.MaxHP * 0.2))
end
  
function Statuses.BadlyHurt:Talk(talker, activator)
  UI:SetSpeaker(talker)
  --#TODO: pick a random line fitting the kind of personality, specie, and etc we have
  UI:WaitShowDialogue(STRINGS:Format("ack.. I-I'm feeling faint.."))
  self.AlreadySaid = true
end
  
function Statuses.BadlyHurt:Relevance(talker) 
  return 0
end

--------------------------------------------------------
-- Belly updates
--------------------------------------------------------

--[[-----------------------------------------------------
    Peckish dialogue
      Says something about how they are feeling a bit hungry
--]]-----------------------------------------------------
Statuses.Peckish = Class("Peckish", BaseLine)

function Statuses.Peckish:initialize()
  self.AlreadySaid = false
end

function Statuses.Peckish:Predicate(talker)
  return talker.Belly <= (talker.MaxBelly / 2)
end
  
function Statuses.Peckish:Talk(talker, activator)
  UI:SetSpeaker(talker)
  --#TODO: pick a random line fitting the kind of personality, specie, and etc we have
  UI:WaitShowDialogue(STRINGS:Format("I wonder what's for dinner?"))
end
  
function Statuses.Peckish:Relevance(talker) 
  return 10 --Low priority
end

--[[-----------------------------------------------------
    Hungry dialogue
      Says something about how they are hungry
--]]-----------------------------------------------------
Statuses.Hungry = Class("Hungry", BaseLine)

function Statuses.Hungry:initialize()
end

function Statuses.Hungry:Predicate(talker)
  return (talker.Belly <= (talker.MaxBelly / 2)) and (talker.Belly > (talker.MaxBelly * 0.2))
end
  
function Statuses.Hungry:Talk(talker, activator)
  UI:SetSpeaker(talker)
  --#TODO: pick a random line fitting the kind of personality, specie, and etc we have
  UI:WaitShowDialogue(STRINGS:Format("I could use something to eat.."))
end

function Statuses.Hungry:Relevance(talker) 
  return 8 --Low priority
end

--[[-----------------------------------------------------
    Famished dialogue
      Says something about how they are very hungry and will soon starve
--]]-----------------------------------------------------
Statuses.Famished = Class("Famished", BaseLine)

function Statuses.Famished:initialize()
end

function Statuses.Famished:Predicate(talker)
  return talker.Belly <= 0
end

function Statuses.Famished:Talk(talker, activator)
  UI:SetSpeaker(talker)
  --#TODO: pick a random line fitting the kind of personality, specie, and etc we have
  UI:WaitShowDialogue(STRINGS:Format("My stomach is growling and I'm feeling weaker.. Can I get something to eat please?"))
end

function Statuses.Famished:Relevance(talker) 
  return 1
end

--[[-----------------------------------------------------
    Starving dialogue
      Says something about how they are starving
--]]-----------------------------------------------------
Statuses.Starving = Class("Starving", BaseLine)

function Statuses.Starving:initialize()
end

function Statuses.Starving:Predicate(talker)
  return talker.Belly <= 0
end

function Statuses.Starving:Talk(talker, activator)
  UI:SetSpeaker(talker)
  --#TODO: pick a random line fitting the kind of personality, specie, and etc we have
  UI:WaitShowDialogue(STRINGS:Format("auugh.. I'm starving.. I gotta eat something or soon I'll be a goner.."))
end

function Statuses.Starving:Relevance(talker) 
  return 0
end

--------------------------------------------------------
-- Status Afflictions
--------------------------------------------------------

--[[-----------------------------------------------------
    Poisoned dialogue
      Says something about how they got poisoned
--]]-----------------------------------------------------
Statuses.Poisoned = Class("Poisoned", BaseLine)

function Statuses.Poisoned:initialize()
end

function Statuses.Poisoned:Predicate(talker)
  for k,_ in pairs(talker.StatusEffects) do
    local entry = PMDOrigins.Data.DataManager.Instance:GetStatus(k)
    if entry.Name.DefaultText == "Poisoned" then
      return true
    end
  end
  return false
end

function Statuses.Poisoned:Talk(talker, activator)
  UI:SetSpeaker(talker)
  --#TODO: pick a random line fitting the kind of personality, specie, and etc we have
  UI:WaitShowDialogue(STRINGS:Format("Urk.. I've been poisoned!"))
end

function Statuses.Poisoned:Relevance(talker) 
  return 0
end

--------------------------------------------------------
-- Low PP
--------------------------------------------------------

-- Return the table
return Statuses