Introduction:

Sprites and portraits are mass imported from SpriteCollab and funneled into the Sprites and Portraits folder in RawAsset.  It uses transfer.json to determine which sprites to move and where.
It also grabs from a Custom folder using custom_transfer.json

Run sprite_sync.py to initiate.  This script automatically updates transfer.json to include newly added nodes from the Spritebot's tracker.json, but does not touch custom_transfer.json
It prints out the diffs that were detected from each import.
It does not do deletions; only replacements.