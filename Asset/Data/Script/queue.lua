--an implementation of a queue was needed for partner AI to remember 
Queue = {}
function Queue.new ()
  return {first = 0, last = -1}
end

--insert to the back ot the line
function Queue.pushleft (queue, value)
  local first = queue.first - 1
  queue.first = first
  queue[first] = value
end

--insert to front of the line
function Queue.pushright (queue, value)
  local last = queue.last + 1
  queue.last = last
  queue[last] = value
end

--take from the back of the line
function Queue.popleft (queue)
  local first = queue.first
  if first > queue.last then error("queue is empty") end
  local value = queue[first]
  queue[first] = nil        -- to allow garbage collection
  queue.first = first + 1
  return value
end


--take from the front of the line 
function Queue.popright (queue)
  local last = queue.last
  if queue.first > last then error("queue is empty") end
  local value = queue[last]
  queue[last] = nil         -- to allow garbage collection
  queue.last = last - 1
  return value
end

return Queue