require 'common'
--------------------------------------------------------
-- Base Class for a line
--------------------------------------------------------
local BaseLine = Class('BaseLine')

function BaseLine:initialize()
end

function BaseLine:Predicate(talker)
  return false
end

function BaseLine:Talk(talker, activator)
end

function BaseLine:Relevance(talker)
  return 0
end

return BaseLine