--[[
  Base class for defining topics 
--]]
require 'common'
BaseTopic = Class('BaseTopic')

--[[
  Constructor
--]]
function BaseTopic:initialize()
  --To implement!
end

--[[
    OnLoad
      Called when a topic should cache its resources
--]]
function BaseTopic:OnLoad()
  --To implement!
end

--[[
    HasSomethingToSay
      Called whenever the character is asked to talk. 
      This checks to see if in the current context we have something to say.
      Then depending on our priority value, the Talk method may be called after by the client code.
      
      returns true or false
--]]
function BaseTopic:HasSomethingToSay(talker, activator)
  --To implement!
  return false
end

--[[
    Talk
      Called when dialog should be generated.
      The method itself is called as a coroutine, so its possible to use usual choregraphy functions used within
      level script files in here as well.
--]]
function BaseTopic:Talk(talker, activator)
  --To implement!
  assert(false,"BaseTopic:Talk(): Base method Talk was not implemented for the topic object " .. tostring(self) .. "!")
end

return BaseTopic